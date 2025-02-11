﻿using RMC.Mini.Service;
using UnityEngine;
using UnityEngine.Assertions;

namespace RMC.Mini.Features.SceneSystem
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class SceneSystemFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            //Model
            SceneSystemModel sceneSystemModel = new SceneSystemModel();
            sceneSystemModel.Initialize(MiniMvcs.Context);

            // Get from scene
            SceneSystemView sceneSystemView = Object.FindFirstObjectByType<SceneSystemView>();
            Assert.IsNotNull(sceneSystemView, $"Add the '{nameof(SceneSystemView)}' Prefab to the scene first.");
   
            // Create new controller
            SceneSystemController controller = new SceneSystemController
            (
                sceneSystemModel, 
                sceneSystemView, 
                new DummyService()
            );
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(sceneSystemView);
            
            // Initialize
            sceneSystemView.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ModelLocator.HasItem<SceneSystemModel>())
            {
                MiniMvcs.ModelLocator.RemoveAndDisposeItem<SceneSystemModel>();
            }
            
            if (MiniMvcs.ViewLocator.HasItem<SceneSystemView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<SceneSystemView>();
            }
            
            if (MiniMvcs.ControllerLocator.HasItem<SceneSystemController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<SceneSystemController>();
            }
            
        }
    }
}
