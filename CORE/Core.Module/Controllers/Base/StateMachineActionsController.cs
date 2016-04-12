using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;

namespace DevExpress.ExpressApp.StateMachine
{
    public class StateMachineActionsController : StateMachineControllerBase<ObjectView>
    {
        private const string EditModeKey = "ViewIsInEditMode";
        private const string SecurityKey = "EnabledBySecurity";

        private readonly Dictionary<object, List<SimpleAction>> panelActions =
            new Dictionary<object, List<SimpleAction>>();

        private IList<ITransition> originalTransitionsList;

        public StateMachineActionsController()
        {
            ChangeStateAction = new SingleChoiceAction(this, "CustomChangeStateAction", PredefinedCategory.Edit);
            ChangeStateAction.ImageName = "Action_StateMachine";
            ChangeStateAction.PaintStyle = ActionItemPaintStyle.Image;
            ChangeStateAction.Caption = "Change State";
            ChangeStateAction.ToolTip = "Change state of the current object";
            ChangeStateAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ChangeStateAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            ChangeStateAction.Execute += stateMachineActivity_Execute;
        }

        public SingleChoiceAction ChangeStateAction { get; }

        public event EventHandler<ExecuteTransitionEventArgs> TransitionExecuting;

        public event EventHandler<ExecuteTransitionEventArgs> TransitionExecuted;

        private void View_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void stateMachineActivity_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var transition = e.SelectedChoiceActionItem.Data as ITransition;
            if (transition == null)
                return;
            ExecuteTransition(e.CurrentObject, transition);
        }

        private void SimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var selectedItem = e.Action.Tag as ChoiceActionItem;
            if (selectedItem == null)
                return;
            ChangeStateAction.DoExecute(selectedItem);
        }

        private int TransitionsComparison(ITransition t1, ITransition t2)
        {
            var transitionUiSettings1 = t1 as ITransitionUISettings;
            var transitionUiSettings2 = t2 as ITransitionUISettings;
            if (transitionUiSettings1 == null)
            {
                return transitionUiSettings2 == null ? 0 : 1;
            }
            if (transitionUiSettings2 == null)
                return -1;
            if (originalTransitionsList != null && transitionUiSettings1.Index == 0 && transitionUiSettings2.Index == 0)
                return originalTransitionsList.IndexOf(t1) - originalTransitionsList.IndexOf(t2);
            return transitionUiSettings1.Index - transitionUiSettings2.Index;
        }

        private IEnumerable<ITransition> GetOrderedTransitions(IList<ITransition> transitionsList)
        {
            var list = new List<ITransition>(transitionsList);
            originalTransitionsList = transitionsList;
            list.Sort(TransitionsComparison);
            originalTransitionsList = null;
            return list;
        }

        private void ResetActionsPanel()
        {
            var detailView = View as DetailView;
            if (detailView != null)
            {
                foreach (string id in panelActions.Keys)
                {
                    var containerViewItem = detailView.FindItem(id) as ActionContainerViewItem;
                    if (containerViewItem != null)
                        containerViewItem.Clear();
                }
                foreach (var list in panelActions.Values)
                {
                    foreach (var simpleAction in list)
                        simpleAction.Execute -= SimpleAction_Execute;
                }
            }
            panelActions.Clear();
        }

        private void IntializeActionsPanel(IStateMachine stateMachine, ChoiceActionItemCollection transitionItems)
        {
            var list = new List<SimpleAction>();
            foreach (var transitionItem in transitionItems)
            {
                var transitionAction = CreateSimpleTransitionAction(stateMachine, transitionItem);
                list.Add(transitionAction);
            }

            //string actionsContainerId = "StateMachineActionContainer_" + stateMachine.Name.Replace( " ", "_" );
            //panelActions[ actionsContainerId ] = list;
            //AddStateMachineActionsContainerToDetailViewLayout( ( DetailView ) View, actionsContainerId, stateMachine.Name );
            //this.Actions.AddRange( list );
        }

        private SimpleAction CreateSimpleTransitionAction(IStateMachine stateMachine, ChoiceActionItem transitionItem)
        {
            var simpleAction = new SimpleAction(this, Guid.NewGuid().ToString(), PredefinedCategory.Edit
                /*, "StateMachineActions"*/);
            simpleAction.Enabled["ViewIsInEditMode"] = ((DetailView) View).ViewEditMode == ViewEditMode.Edit;
            simpleAction.Enabled["EnabledBySecurity"] = DataManipulationRight.CanEdit(stateMachine.TargetObjectType,
                stateMachine.StatePropertyName, View.CurrentObject, null, View.ObjectSpace);
            simpleAction.Tag = transitionItem;
            simpleAction.Caption = transitionItem.Caption;
            simpleAction.Execute += SimpleAction_Execute;
            return simpleAction;
        }

        private void AddStateMachineActionsContainerToDetailViewLayout(DetailView detailView, string actionsContainerId,
            string caption)
        {
            if (detailView.Model.Items[actionsContainerId] != null)
                return;
            var containerViewItem1 = detailView.Model.Items.AddNode<IModelActionContainerViewItem>(actionsContainerId);
            var modelApplicationBase = (ModelApplicationBase) detailView.Model.Application;
            var currentAspect = modelApplicationBase.CurrentAspect;
            modelApplicationBase.SetCurrentAspect("");
            containerViewItem1.Caption = caption;
            modelApplicationBase.SetCurrentAspect(currentAspect);
            var viewLayoutElement = detailView.Model.Layout.Count > 0 ? detailView.Model.Layout[0] : null;
            var modelLayoutViewItem = !(viewLayoutElement is IModelLayoutGroup)
                ? detailView.Model.Layout.AddNode<IModelLayoutViewItem>(containerViewItem1.Id)
                : viewLayoutElement.AddNode<IModelLayoutViewItem>(containerViewItem1.Id);
            modelLayoutViewItem.ViewItem = containerViewItem1;
            modelLayoutViewItem.ShowCaption = true;
            var containerViewItem2 = (ActionContainerViewItem) detailView.AddItem(containerViewItem1);
        }

        private void detailView_ViewEditModeChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void View_InfoChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            var list = new List<string>();
            foreach (var stateMachine in GetStateMachines())
                list.Add(stateMachine.StatePropertyName);
            if (list.Count <= 0 || e.Object != View.CurrentObject || !list.Contains(e.PropertyName))
                return;
            UpdateActionState();
        }

        protected internal virtual void UpdateActionState()
        {
            if (!StateMachineCacheController.Active)
            {
                StateMachineCacheController.Activated += StateMachineCacheController_Activated;
            }
            else
            {
                ChangeStateAction.Items.Clear();
                var detailView = View as DetailView;
                ISupportUpdate supportUpdate = null;
                try
                {
                    if (detailView != null)
                    {
                        supportUpdate = detailView.LayoutManager.Container as ISupportUpdate;
                        if (supportUpdate != null)
                            supportUpdate.BeginUpdate();
                        ResetActionsPanel();
                    }
                    var targetObject = View.SelectedObjects.Count == 1 ? View.SelectedObjects[0] : null;
                    if (targetObject != null)
                    {
                        foreach (var stateMachine in GetStateMachines())
                        {
                            var currentState = stateMachine.FindCurrentState(targetObject);
                            if (currentState != null && currentState.Transitions.Count > 0)
                            {
                                var choiceActionItem = new ChoiceActionItem(stateMachine.Name, stateMachine);
                                choiceActionItem.Enabled["EnabledBySecurity"] =
                                    DataManipulationRight.CanEdit(stateMachine.TargetObjectType,
                                        stateMachine.StatePropertyName, targetObject, null, View.ObjectSpace);
                                ChangeStateAction.Items.Add(choiceActionItem);
                                foreach (var transition in GetOrderedTransitions(currentState.Transitions))
                                    choiceActionItem.Items.Add(new ChoiceActionItem(transition.Caption, transition));
                                if (View is DetailView && stateMachine is IStateMachineUISettings &&
                                    ((IStateMachineUISettings) stateMachine).ExpandActionsInDetailView)
                                    IntializeActionsPanel(stateMachine, choiceActionItem.Items);
                            }
                        }
                    }
                    if (detailView != null)
                    {
                        ChangeStateAction.Enabled["ViewIsInEditMode"] = detailView.ViewEditMode == ViewEditMode.Edit;
                        var flag = true;
                        foreach (string id in panelActions.Keys)
                        {
                            if (!(detailView.FindItem(id) is ActionContainerViewItem))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            RegisterActionsInPanelContainers(detailView);
                        }
                        else
                        {
                            if (!detailView.IsControlCreated)
                                return;
                            View.BreakLinksToControls();
                            View.LoadModel();
                            View.CreateControls();
                        }
                    }
                    else
                        ChangeStateAction.Enabled.RemoveItem("ViewIsInEditMode");
                }
                finally
                {
                    if (supportUpdate != null)
                        supportUpdate.EndUpdate();
                }
            }
        }

        private void StateMachineCacheController_Activated(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void RegisterActionsInPanelContainers(DetailView detailView)
        {
            foreach (string id in panelActions.Keys)
            {
                var containerViewItem = detailView.FindItem(id) as ActionContainerViewItem;
                if (containerViewItem != null)
                {
                    containerViewItem.Clear();
                    foreach (var simpleAction in panelActions[id])
                        containerViewItem.Register(simpleAction);
                }
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            var detailView = View as DetailView;
            if (detailView == null)
                return;
            RegisterActionsInPanelContainers(detailView);
        }

        protected internal void ExecuteTransition(object targetObject, ITransition transition)
        {
            var args = new ExecuteTransitionEventArgs(targetObject, transition);
            OnStateMachineTransitionExecuting(args);
            if (args.Cancel)
                return;
            transition.TargetState.StateMachine.ExecuteTransition(targetObject, transition.TargetState);
            ObjectSpace.SetModified(targetObject);
            OnStateMachineTransitionExecuted(args);
            if ((transition as ITransitionUISettings).SaveAndCloseView)
            {
                View.ObjectSpace.CommitChanges();
                View.Close();
            }
            if (!(View is ListView) || Frame == null)
                return;
            var controller = Frame.GetController<ModificationsController>();
            if (controller == null || controller.ModificationsHandlingMode != ModificationsHandlingMode.AutoCommit)
                return;
            View.ObjectSpace.CommitChanges();
        }

        protected virtual void OnStateMachineTransitionExecuting(ExecuteTransitionEventArgs args)
        {
            if (TransitionExecuting == null)
                return;
            TransitionExecuting(this, args);
        }

        protected virtual void OnStateMachineTransitionExecuted(ExecuteTransitionEventArgs args)
        {
            if (TransitionExecuted == null)
                return;
            TransitionExecuted(this, args);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.SelectionChanged += View_SelectionChanged;
            View.ControlsCreated += View_ControlsCreated;
            View.ModelChanged += View_InfoChanged;
            View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            var detailView = View as DetailView;
            if (detailView != null)
                detailView.ViewEditModeChanged += detailView_ViewEditModeChanged;
            UpdateActionState();
        }

        protected override void OnDeactivated()
        {
            var detailView = View as DetailView;
            if (detailView != null)
                detailView.ViewEditModeChanged -= detailView_ViewEditModeChanged;
            View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            View.ModelChanged -= View_InfoChanged;
            View.ControlsCreated -= View_ControlsCreated;
            View.SelectionChanged -= View_SelectionChanged;
            StateMachineCacheController.Activated -= StateMachineCacheController_Activated;
            base.OnDeactivated();
        }
    }
}