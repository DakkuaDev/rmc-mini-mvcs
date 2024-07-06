using System;
using RMC.Mini.View;
using RMC.Mini.Samples.UGS.Mini.Controller;
using RMC.Mini.Samples.UGS.Mini.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Mini.Samples.UGS.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="HudController"/>
    /// 
    /// </summary>
    public class HudView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnBack = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnDeveloperConsole = new UnityEvent();

        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Label StatusLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("StatusLabel"); }}
        public Button BackButton { get { return _uiDocument?.rootVisualElement.Q<Button>("BackButton"); }}
        public Button DeveloperConsoleButton { get { return _uiDocument?.rootVisualElement.Q<Button>("DeveloperConsoleButton"); }}
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                BackButton.clicked += BackButton_OnClicked;
                DeveloperConsoleButton.clicked += DeveloperConsoleButtonOnClicked;
                
                RefreshUI();
                
            }
        }

        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------
        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        public void RefreshUI()
        {
            UgsModel model = Context.ModelLocator.GetItem<UgsModel>();
            StatusLabel.text = SceneManager.GetActiveScene().name;
            BackButton.SetEnabled(model.SceneHasNavigationBack.Value);
            DeveloperConsoleButton.SetEnabled(model.SceneHasNavigationDeveloperConsole.Value);

        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void BackButton_OnClicked()
        {
            OnBack.Invoke();
        }

        private void DeveloperConsoleButtonOnClicked()
        {
            OnDeveloperConsole.Invoke();
        }
    }
}