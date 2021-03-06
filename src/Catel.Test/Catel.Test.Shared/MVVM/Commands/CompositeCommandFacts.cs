﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeCommandFacts.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Catel.Test.MVVM
{
    using System;
    using Catel.MVVM;

#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public class CompositeCommandFacts
    {
        [TestClass]
        public class TheRegisterCommandMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullCommand()
            {
                var compositeCommand = new CompositeCommand();

                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => compositeCommand.RegisterCommand(null));
            }

            [TestMethod]
            public void RegistersCommandForExecution()
            {
                var vm = new CompositeCommandViewModel();
                var compositeCommand = new CompositeCommand(); 

                compositeCommand.RegisterCommand(vm.TestCommand1, vm);

                compositeCommand.Execute();

                Assert.IsTrue(vm.IsTestCommand1Executed);
            }
        }

        [TestClass]
        public class TheUnregisterCommandMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullCommand()
            {
                var compositeCommand = new CompositeCommand();

                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => compositeCommand.UnregisterCommand(null));
            }

            [TestMethod]
            public void UnregistersCommandForExecution()
            {
                var vm = new CompositeCommandViewModel();
                var compositeCommand = new CompositeCommand(); 

                compositeCommand.RegisterCommand(vm.TestCommand1, vm);
                compositeCommand.RegisterCommand(vm.TestCommand2, vm);

                compositeCommand.UnregisterCommand(vm.TestCommand1);

                compositeCommand.Execute();

                Assert.IsFalse(vm.IsTestCommand1Executed);
                Assert.IsTrue(vm.IsTestCommand2Executed);
            }
        }

        [TestClass]
        public class TheRegisterGenericActionMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullAction()
            {
                var compositeCommand = new CompositeCommand();

                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => compositeCommand.RegisterAction((Action<object>)null));
            }

            [TestMethod]
            public void RegistersActionForExecution()
            {
                var compositeCommand = new CompositeCommand();

                bool executed = false;
                var action = new Action<object>(obj => executed = true);

                compositeCommand.RegisterAction(action);
                compositeCommand.Execute();

                Assert.IsTrue(executed);
            }
        }

        [TestClass]
        public class TheUnregisterGenericActionMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullAction()
            {
                var compositeCommand = new CompositeCommand();

                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => compositeCommand.UnregisterAction((Action<object>)null));
            }

            [TestMethod]
            public void UnregistersCommandForExecution()
            {
                var compositeCommand = new CompositeCommand();

                bool executed = false;
                var action = new Action<object>(obj => executed = true);

                compositeCommand.RegisterAction(action);
                compositeCommand.UnregisterAction(action);

                compositeCommand.Execute();

                Assert.IsFalse(executed);
            }
        }

        [TestClass]
        public class TheAutoUnsubscribeFunctionality
        {
            [TestMethod]
            public void AutomaticallyUnsubscribesCommandOnViewModelClosed()
            {
                var vm = new CompositeCommandViewModel();
                var compositeCommand = new CompositeCommand();

                compositeCommand.RegisterCommand(vm.TestCommand1, vm);

                Assert.IsFalse(vm.IsTestCommand1Executed);

                vm.CloseViewModel(false);

                compositeCommand.Execute();

                Assert.IsFalse(vm.IsTestCommand1Executed);
            }
        }
    }
}