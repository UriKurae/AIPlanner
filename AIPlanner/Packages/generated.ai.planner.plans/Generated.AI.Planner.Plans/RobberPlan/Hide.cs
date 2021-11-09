using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.RobberPlan;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Generated.AI.Planner.Plans.RobberPlan
{
    [BurstCompile]
    struct Hide : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_RobberIndex = 0;
        const int k_TreeIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Robber",
            "Tree",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> RobberFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> RobberObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> TreeFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> TreeObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<HideFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal Hide(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            RobberFilter = default;
            RobberObjectIndices = default;
            TreeFilter = default;
            TreeObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            RobberFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Robber>(),[1] = ComponentType.ReadWrite<Location>(),  };
            RobberObjectIndices = new NativeList<int>(2, Allocator.Temp);
            TreeFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Location>(),[1] = ComponentType.ReadWrite<TreeHide>(),  };
            TreeObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<HideFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Robber", StringComparison.OrdinalIgnoreCase))
                 return k_RobberIndex;
            if (string.Equals(parameterName, "Tree", StringComparison.OrdinalIgnoreCase))
                 return k_TreeIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            RobberObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(RobberObjectIndices, RobberFilter);
            
            TreeObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(TreeObjectIndices, TreeFilter);
            
            var RobberBuffer = stateData.RobberBuffer;
            
            

            for (int i0 = 0; i0 < RobberObjectIndices.Length; i0++)
            {
                var RobberIndex = RobberObjectIndices[i0];
                var RobberObject = stateData.TraitBasedObjects[RobberIndex];
                
                if (!(RobberBuffer[RobberObject.RobberIndex].Stolen == true))
                    continue;
                
                if (!(RobberBuffer[RobberObject.RobberIndex].Hide == false))
                    continue;
                
                
            
            

            for (int i1 = 0; i1 < TreeObjectIndices.Length; i1++)
            {
                var TreeIndex = TreeObjectIndices[i1];
                var TreeObject = stateData.TraitBasedObjects[TreeIndex];
                
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_RobberIndex] = RobberIndex,
                                                       [k_TreeIndex] = TreeIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalRobberObject = originalStateObjectBuffer[action[k_RobberIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newRobberBuffer = newState.RobberBuffer;
            {
                    var @Robber = newRobberBuffer[originalRobberObject.RobberIndex];
                    @Robber.@Hide = true;
                    newRobberBuffer[originalRobberObject.RobberIndex] = @Robber;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 0f;

            return reward;
        }

        public void Execute(int jobIndex)
        {
            if (!LocalContainersInitialized)
                InitializeLocalContainers();

            m_StateDataContext.JobIndex = jobIndex;

            var stateEntityKey = m_StatesToExpand[jobIndex];
            var stateData = m_StateDataContext.GetStateData(stateEntityKey);

            ArgumentPermutations.Clear();
            GenerateArgumentPermutations(stateData, ArgumentPermutations);

            TransitionInfo.Clear();
            TransitionInfo.Capacity = math.max(TransitionInfo.Capacity, ArgumentPermutations.Length);
            for (var i = 0; i < ArgumentPermutations.Length; i++)
            {
                TransitionInfo.Add(new HideFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<HideFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetRobberTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_RobberIndex]);
        }
        
        public static T GetTreeTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_TreeIndex]);
        }
        
    }

    public struct HideFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


