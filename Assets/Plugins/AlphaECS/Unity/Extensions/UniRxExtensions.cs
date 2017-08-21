using System; // require keep for Windows Universal App
using UnityEngine;
using System.Linq;

#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)
using UnityEngine.EventSystems;
#endif

namespace UniRx.Triggers
{
	public static partial class ObservableTriggerExtensions
	{
		public static IObservable<BaseEventData> OnDeselectAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableDeselectTrigger>(component.gameObject).OnDeselectAsObservable();
		}

		public static IObservable<AxisEventData> OnMoveAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<AxisEventData>();
			return GetOrAddComponent<ObservableMoveTrigger>(component.gameObject).OnMoveAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerDownAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerDownTrigger>(component.gameObject).OnPointerDownAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerEnterAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerEnterTrigger>(component.gameObject).OnPointerEnterAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerExitAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerExitTrigger>(component.gameObject).OnPointerExitAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerUpAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerUpTrigger>(component.gameObject).OnPointerUpAsObservable();
		}

		public static IObservable<BaseEventData> OnSelectAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableSelectTrigger>(component.gameObject).OnSelectAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerClickAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerClickTrigger>(component.gameObject).OnPointerClickAsObservable();
		}

		public static IObservable<BaseEventData> OnSubmitAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableSubmitTrigger>(component.gameObject).OnSubmitAsObservable();
		}

		public static IObservable<PointerEventData> OnDragAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableDragTrigger>(component.gameObject).OnDragAsObservable();
		}

		public static IObservable<PointerEventData> OnBeginDragAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableBeginDragTrigger>(component.gameObject).OnBeginDragAsObservable();
		}

		public static IObservable<PointerEventData> OnEndDragAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableEndDragTrigger>(component.gameObject).OnEndDragAsObservable();
		}

		public static IObservable<PointerEventData> OnDropAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableDropTrigger>(component.gameObject).OnDropAsObservable();
		}

		public static IObservable<BaseEventData> OnUpdateSelectedAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableUpdateSelectedTrigger>(component.gameObject).OnUpdateSelectedAsObservable();
		}

		public static IObservable<PointerEventData> OnInitializePotentialDragAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableInitializePotentialDragTrigger>(component.gameObject).OnInitializePotentialDragAsObservable();
		}

		public static IObservable<BaseEventData> OnCancelAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableCancelTrigger>(component.gameObject).OnCancelAsObservable();
		}

		public static IObservable<PointerEventData> OnScrollAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableScrollTrigger>(component.gameObject).OnScrollAsObservable();
		}








		public static IObservable<BaseEventData> OnDeselectAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableDeselectTrigger>(component.gameObject).OnDeselectAsObservable();
		}

		public static IObservable<AxisEventData> OnMoveAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<AxisEventData>();
			return GetOrAddComponent<ObservableMoveTrigger>(component.gameObject).OnMoveAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerDownAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerDownTrigger>(component.gameObject).OnPointerDownAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerEnterAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerEnterTrigger>(component.gameObject).OnPointerEnterAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerExitAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerExitTrigger>(component.gameObject).OnPointerExitAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerUpAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerUpTrigger>(component.gameObject).OnPointerUpAsObservable();
		}

		public static IObservable<BaseEventData> OnSelectAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableSelectTrigger>(component.gameObject).OnSelectAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerClickAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservablePointerClickTrigger>(component.gameObject).OnPointerClickAsObservable();
		}

		public static IObservable<BaseEventData> OnSubmitAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableSubmitTrigger>(component.gameObject).OnSubmitAsObservable();
		}

		public static IObservable<PointerEventData> OnDragAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableDragTrigger>(component.gameObject).OnDragAsObservable();
		}

		public static IObservable<PointerEventData> OnBeginDragAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableBeginDragTrigger>(component.gameObject).OnBeginDragAsObservable();
		}

		public static IObservable<PointerEventData> OnEndDragAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableEndDragTrigger>(component.gameObject).OnEndDragAsObservable();
		}

		public static IObservable<PointerEventData> OnDropAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableDropTrigger>(component.gameObject).OnDropAsObservable();
		}

		public static IObservable<BaseEventData> OnUpdateSelectedAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableUpdateSelectedTrigger>(component.gameObject).OnUpdateSelectedAsObservable();
		}

		public static IObservable<PointerEventData> OnInitializePotentialDragAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableInitializePotentialDragTrigger>(component.gameObject).OnInitializePotentialDragAsObservable();
		}

		public static IObservable<BaseEventData> OnCancelAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<BaseEventData>();
			return GetOrAddComponent<ObservableCancelTrigger>(component.gameObject).OnCancelAsObservable();
		}

		public static IObservable<PointerEventData> OnScrollAsObservable(this GameObject component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return GetOrAddComponent<ObservableScrollTrigger>(component.gameObject).OnScrollAsObservable();
		}



		public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateEnter(this Animator animator, string fullPath) 
		{

			var observableStateMachineTriggers = animator.GetBehaviours<ObservableStateMachineTrigger>();
			int fullPathHash = Animator.StringToHash(fullPath);

			var observableTriggers = observableStateMachineTriggers.Select(trigger => trigger.OnStateEnterAsObservable()
				.Where(onStateInfo => onStateInfo.StateInfo.fullPathHash == fullPathHash));

			return Observable.Merge(observableTriggers);
		}

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateUpdate(this Animator animator, params string[] fullPaths) 
        {
			var fullPathHashes = fullPaths.Select(s => Animator.StringToHash(s)).ToArray();
			return animator.OnStateUpdate (fullPathHashes);
        }

		public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateUpdate(this Animator animator, params int[] fullPathHashes) 
		{
			var observableStateMachineTriggers = animator.GetBehaviours<ObservableStateMachineTrigger>();

			var observableTriggers = observableStateMachineTriggers.Select(trigger => trigger.OnStateUpdateAsObservable()
				.Where(onStateInfo => fullPathHashes.Contains(onStateInfo.StateInfo.fullPathHash)));

			return Observable.Merge(observableTriggers);
		}

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateNormalizedTime(this Animator animator, string fullPath, float normalizedTime) 
        {

            var observableStateMachineTriggers = animator.GetBehaviours<ObservableStateMachineTrigger>();
            int fullPathHash = Animator.StringToHash(fullPath);

            var emit = false;

            var observableTriggers = observableStateMachineTriggers.Select(trigger => trigger.OnStateUpdateAsObservable()
                .Where(onStateInfo => 
                {
                    if (onStateInfo.StateInfo.fullPathHash == fullPathHash)
                    {
                        if (emit)
                        {
                            if (onStateInfo.StateInfo.normalizedTime >= normalizedTime)
                            {
                                emit = false;
                                return true;
                            }
                        }
                        else
                        {
                            if (onStateInfo.StateInfo.normalizedTime < normalizedTime)
                            {
                                emit = true;
                            }
                        }

                    }

                    return false;
                }));

            return Observable.Merge(observableTriggers);
        }

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateExit(this Animator animator, string fullPath) 
        {

            var observableStateMachineTriggers = animator.GetBehaviours<ObservableStateMachineTrigger>();
            int fullPathHash = Animator.StringToHash(fullPath);

            var observableTriggers = observableStateMachineTriggers.Select(trigger => trigger.OnStateExitAsObservable()
                .Where(onStateInfo => onStateInfo.StateInfo.fullPathHash == fullPathHash));

            return Observable.Merge(observableTriggers);
        }
	}
}
