using System;
using Avalonia;
using Avalonia.Controls;
using CommonServiceLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity.Wpf.Tests;
using Unity.Lifetime;
using Moq;

namespace Prism.Unity.Avalonia.Tests
{
    [TestClass]
    public class UnityBootstrapperRunMethodFixture
    {
        [TestMethod]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
        }

        [TestMethod]
        public void RunShouldNotFailIfReturnedNullShell()
        {
            var bootstrapper = new DefaultUnityBootstrapper { ShellObject = null };
            bootstrapper.Run();
        }

        [TestMethod]
        public void RunConfiguresServiceLocatorProvider()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(CommonServiceLocator.ServiceLocator.Current is UnityServiceLocatorAdapter);
        }

        [TestMethod]
        public void RunShouldInitializeContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            var container = bootstrapper.BaseContainer;

            Assert.IsNull(container);

            bootstrapper.Run();

            container = bootstrapper.BaseContainer;

            Assert.IsNotNull(container);
            Assert.IsInstanceOfType(container,typeof(UnityContainer));
        }

        [TestMethod]
        public void RunAddsCompositionContainerToContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            var createdContainer = bootstrapper.CallCreateContainer();
            var returnedContainer = createdContainer.Resolve<IUnityContainer>();
            Assert.IsNotNull(returnedContainer);
            Assert.IsInstanceOfType(returnedContainer,typeof(IUnityContainer));
        }

        [TestMethod]
        public void RunShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.InitializeModulesCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureDefaultRegionBehaviors()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureDefaultRegionBehaviorsCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureRegionAdapterMappings()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureRegionAdapterMappingsCalled);
        }

        [TestMethod]
        public void RunShouldAssignRegionManagerToReturnedShell()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsNotNull(RegionManager.GetRegionManager(bootstrapper.BaseShell));
        }

        [TestMethod]
        public void RunShouldCallCreateLogger()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateLoggerCalled);
        }

        [TestMethod]
        public void RunShouldCallCreateModuleCatalog()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateModuleCatalogCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureModuleCatalog()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureModuleCatalogCalled);
        }

        [TestMethod]
        public void RunShouldCallCreateContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateContainerCalled);
        }

        [TestMethod]
        public void RunShouldCallCreateShell()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateShellCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureContainer()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureContainerCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureServiceLocator()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureServiceLocatorCalled);
        }

        [TestMethod]
        public void RunShouldCallConfigureViewModelLocator()
        {
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureViewModelLocatorCalled);
        }

        [TestMethod]
        public void RunRegistersInstanceOfILoggerFacade()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);


            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterInstance(typeof(ILoggerFacade), null, bootstrapper.BaseLogger, It.IsAny<IInstanceLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersInstanceOfIModuleCatalog()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterInstance(typeof(IModuleCatalog), null, It.IsAny<object>(), It.IsAny<IInstanceLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForIServiceLocator()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(IServiceLocator), typeof(UnityServiceLocatorAdapter), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForIModuleInitializer()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(IModuleInitializer), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForIRegionManager()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(IRegionManager), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForRegionAdapterMappings()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(RegionAdapterMappings), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForIRegionViewRegistry()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(IRegionViewRegistry), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForIRegionBehaviorFactory()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(IRegionBehaviorFactory), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunRegistersTypeForIEventAggregator()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.RegisterType(typeof(IEventAggregator), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Once());
        }

        [TestMethod]
        public void RunFalseShouldNotRegisterDefaultServicesAndTypes()
        {
            var mockedContainer = new Mock<IUnityContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);
            bootstrapper.Run(false);

            mockedContainer.Verify(c => c.RegisterType(typeof(IEventAggregator), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Never());
            mockedContainer.Verify(c => c.RegisterType(typeof(IRegionManager), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Never());
            mockedContainer.Verify(c => c.RegisterType(typeof(RegionAdapterMappings), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Never());
            mockedContainer.Verify(c => c.RegisterType(typeof(IServiceLocator), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Never());
            mockedContainer.Verify(c => c.RegisterType(typeof(IModuleInitializer), It.IsAny<Type>(), null, It.IsAny<ITypeLifetimeManager>()), Times.Never());
        }

        [TestMethod]
        public void ModuleManagerRunCalled()
        {
            // Have to use a non-mocked container because of IsRegistered<> extension method, Registrations property,and ContainerRegistration
            var container = new UnityContainer();

            var mockedModuleInitializer = new Mock<IModuleInitializer>();
            var mockedModuleManager = new Mock<IModuleManager>();
            var regionAdapterMappings = new RegionAdapterMappings();
            var serviceLocatorAdapter = new UnityServiceLocatorAdapter(container);
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocatorAdapter);

            container.RegisterInstance<IServiceLocator>(serviceLocatorAdapter);
            container.RegisterInstance<IModuleCatalog>(new ModuleCatalog());
            container.RegisterInstance<IModuleInitializer>(mockedModuleInitializer.Object);
            container.RegisterInstance<IModuleManager>(mockedModuleManager.Object);
            container.RegisterInstance<RegionAdapterMappings>(regionAdapterMappings);
            
            container.RegisterSingleton(typeof(RegionAdapterMappings), typeof(RegionAdapterMappings));
            container.RegisterSingleton(typeof(IRegionManager), typeof(RegionManager));
            container.RegisterSingleton(typeof(IEventAggregator), typeof(EventAggregator));
            container.RegisterSingleton(typeof(IRegionViewRegistry), typeof(RegionViewRegistry));
            container.RegisterSingleton(typeof(IRegionBehaviorFactory), typeof(RegionBehaviorFactory));
            container.RegisterSingleton(typeof(IRegionNavigationJournalEntry), typeof(RegionNavigationJournalEntry));
            container.RegisterSingleton(typeof(IRegionNavigationJournal), typeof(RegionNavigationJournal));
            container.RegisterSingleton(typeof(IRegionNavigationService), typeof(RegionNavigationService));
            container.RegisterSingleton(typeof(IRegionNavigationContentLoader), typeof(Regions.UnityRegionNavigationContentLoader));


//            container.RegisterInstance<SelectorRegionAdapter>(new SelectorRegionAdapter(regionBehaviorFactory));
            container.RegisterInstance<ItemsControlRegionAdapter>(new ItemsControlRegionAdapter(regionBehaviorFactory));
            container.RegisterInstance<ContentControlRegionAdapter>(new ContentControlRegionAdapter(regionBehaviorFactory));

            var bootstrapper = new MockedContainerBootstrapper(container);
            bootstrapper.Run(false);

            mockedModuleManager.Verify(mm => mm.Run(), Times.Once());
        }

        [TestMethod]
        public void RunShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MethodCalls[0].Contains("CreateLogger"));
            Assert.IsTrue(bootstrapper.MethodCalls[1].Contains("CreateModuleCatalog"));
            Assert.IsTrue(bootstrapper.MethodCalls[2].Contains("ConfigureModuleCatalog"));
            Assert.IsTrue(bootstrapper.MethodCalls[3].Contains("CreateContainer"));
            Assert.IsTrue(bootstrapper.MethodCalls[4].Contains("ConfigureContainer"));
            Assert.IsTrue(bootstrapper.MethodCalls[5].Contains("ConfigureServiceLocator"));
            Assert.IsTrue(bootstrapper.MethodCalls[6].Contains("ConfigureViewModelLocator"));
            Assert.IsTrue(bootstrapper.MethodCalls[7].Contains("ConfigureRegionAdapterMappings"));
            Assert.IsTrue(bootstrapper.MethodCalls[8].Contains("ConfigureDefaultRegionBehaviors"));
            Assert.IsTrue(bootstrapper.MethodCalls[9].Contains("RegisterFrameworkExceptionTypes"));
            Assert.IsTrue(bootstrapper.MethodCalls[10].Contains("CreateShell"));
            Assert.IsTrue(bootstrapper.MethodCalls[11].Contains("InitializeShell"));
            Assert.IsTrue(bootstrapper.MethodCalls[12].Contains("InitializeModules"));
        }

        [TestMethod]
        public void RunShouldLogBootstrapperSteps()
        {
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue("Logger was created successfully." == messages[0]);
            Assert.IsTrue("Creating module catalog." == messages[1]);
            Assert.IsTrue("Configuring module catalog." == messages[2]);
            Assert.IsTrue("Creating Unity container." == messages[3]);
            Assert.IsTrue("Configuring the Unity container." == messages[4]);
            Assert.IsTrue("Adding UnityBootstrapperExtension to container." == messages[5]);
            Assert.IsTrue("Configuring ServiceLocator singleton." == messages[6]);
            Assert.IsTrue("Configuring the ViewModelLocator to use Unity." == messages[7]);
            Assert.IsTrue("Configuring region adapters." == messages[8]);
            Assert.IsTrue("Configuring default region behaviors." == messages[9]);
            Assert.IsTrue("Registering Framework Exception Types." == messages[10]);
            Assert.IsTrue("Creating the shell." == messages[11]);
            Assert.IsTrue("Setting the RegionManager." == messages[12]);
            Assert.IsTrue("Updating Regions." == messages[13]);
            Assert.IsTrue("Initializing the shell." == messages[14]);
            Assert.IsTrue("Initializing modules." == messages[15]);
            Assert.IsTrue("Bootstrapper sequence completed." == messages[16]);
        }

        [TestMethod]
        public void RunShouldLogLoggerCreationSuccess()
        {
            const string expectedMessageText = "Logger was created successfully.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }
        [TestMethod]
        public void RunShouldLogAboutModuleCatalogCreation()
        {
            const string expectedMessageText = "Creating module catalog.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringModuleCatalog()
        {
            const string expectedMessageText = "Configuring module catalog.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutCreatingTheContainer()
        {
            const string expectedMessageText = "Creating Unity container.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringContainer()
        {
            const string expectedMessageText = "Configuring the Unity container.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringViewModelLocator()
        {
            const string expectedMessageText = "Configuring the ViewModelLocator to use Unity.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringRegionAdapters()
        {
            const string expectedMessageText = "Configuring region adapters.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutConfiguringRegionBehaviors()
        {
            const string expectedMessageText = "Configuring default region behaviors.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutRegisteringFrameworkExceptionTypes()
        {
            const string expectedMessageText = "Registering Framework Exception Types.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutCreatingTheShell()
        {
            const string expectedMessageText = "Creating the shell.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutInitializingTheShellIfShellCreated()
        {
            const string expectedMessageText = "Initializing the shell.";
            var bootstrapper = new DefaultUnityBootstrapper();

            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldNotLogAboutInitializingTheShellIfShellIsNotCreated()
        {
            const string expectedMessageText = "Initializing shell";
            var bootstrapper = new DefaultUnityBootstrapper { ShellObject = null };

            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsFalse(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutInitializingModules()
        {
            const string expectedMessageText = "Initializing modules.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        [TestMethod]
        public void RunShouldLogAboutRunCompleting()
        {
            const string expectedMessageText = "Bootstrapper sequence completed.";
            var bootstrapper = new DefaultUnityBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.BaseLogger.Messages;

            Assert.IsTrue(messages.Contains(expectedMessageText));
        }

        private static void SetupMockedContainerForVerificationTests(Mock<IUnityContainer> mockedContainer)
        {
            var mockedModuleInitializer = new Mock<IModuleInitializer>();
            var mockedModuleManager = new Mock<IModuleManager>();
            var regionAdapterMappings = new RegionAdapterMappings();
            var serviceLocatorAdapter = new UnityServiceLocatorAdapter(mockedContainer.Object);
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocatorAdapter);

            mockedContainer.Setup(c => c.Resolve(typeof(IServiceLocator), (string)null)).Returns(serviceLocatorAdapter);

            mockedContainer.Setup(c => c.RegisterInstance(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IInstanceLifetimeManager>()));

            mockedContainer.Setup(c => c.Resolve(typeof(IModuleCatalog), (string)null)).Returns(
                new ModuleCatalog());

            mockedContainer.Setup(c => c.Resolve(typeof(IModuleInitializer), (string)null)).Returns(
                mockedModuleInitializer.Object);

            mockedContainer.Setup(c => c.Resolve(typeof(IModuleManager), (string)null)).Returns(
                mockedModuleManager.Object);


            mockedContainer.Setup(c => c.Resolve(typeof(RegionAdapterMappings), (string)null)).Returns(
                regionAdapterMappings);

//            mockedContainer.Setup(c => c.Resolve(typeof(SelectorRegionAdapter), (string)null)).Returns(
//                new SelectorRegionAdapter(regionBehaviorFactory));

            mockedContainer.Setup(c => c.Resolve(typeof(ItemsControlRegionAdapter), (string)null)).Returns(
                new ItemsControlRegionAdapter(regionBehaviorFactory));

            mockedContainer.Setup(c => c.Resolve(typeof(ContentControlRegionAdapter), (string)null)).Returns(
                new ContentControlRegionAdapter(regionBehaviorFactory));
        }

        private class MockedContainerBootstrapper : UnityBootstrapper
        {
            private readonly IUnityContainer container;
            public ILoggerFacade BaseLogger
            { get { return base.Logger; } }

            public void CallConfigureContainer()
            {
                base.ConfigureContainer();
            }

            public MockedContainerBootstrapper(IUnityContainer container)
            {
                this.container = container;
            }

            protected override IUnityContainer CreateContainer()
            {
                return container;
            }

            protected override IAvaloniaObject CreateShell()
            {
                return new UserControl();
            }

            protected override void InitializeShell()
            {
                // no op
            }
        }

    }
}
