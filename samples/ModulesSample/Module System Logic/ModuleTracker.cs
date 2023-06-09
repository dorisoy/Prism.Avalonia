﻿// Temp disabled logging:
// - Prism.Logging has been moved and `ILoggerFacade` is deprecated.Prism.Logging.Serilog is out of date.
// - https://github.com/augustoproiete/prism-logging-serilog/issues/3
using ModulesSample.Infrastructure;
////using Prism.Logging;

namespace ModulesSample.Module_System_Logic
{
    class ModuleTracker : IModuleTracker
    {
        ////private ILoggerFacade _logger;

        public ModuleTracker()
        ////public ModuleTracker(ILoggerFacade logger)
        {
            ////if (logger == null)
            ////    throw new ArgumentNullException(nameof(logger));
            ////
            ////_logger = logger;
        }

        public ModuleTrackingState ModuleCodeEditorTrackingState { get; }

        public void RecordModuleLoaded(string moduleName)
        {
            ////_logger.Log(string.Format(CultureInfo.CurrentCulture, "'{0}' module loaded.", moduleName), Category.Debug, Priority.Low);
        }

        public void RecordModuleConstructed(string moduleName)
        {
            ModuleTrackingState moduleTrackingState = GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Constructed;
            }

            ////_logger.Log(string.Format(CultureInfo.CurrentCulture, "'{0}' module constructed.", moduleName), Category.Debug, Priority.Low);
        }

        public void RecordModuleInitialized(string moduleName)
        {
            ModuleTrackingState moduleTrackingState = GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Initialized;
            }

            ////_logger.Log(string.Format(CultureInfo.CurrentCulture, "'{0}' module initialized.", moduleName), Category.Debug, Priority.Low);
        }

        private ModuleTrackingState GetModuleTrackingState(string moduleName)
        {
            switch (moduleName)
            {
                default:
                    return null;
            }
        }
    }
}
