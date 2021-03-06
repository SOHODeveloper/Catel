﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatelDependencyResolverFacts.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Catel.Test.IoC
{
    using System;
    using Catel.IoC;
    using Catel.Services;
#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public class CatelDependencyResolverFacts
    {
        [TestClass]
        public class TheConstructor
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNulServiceLocator()
            {
                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => new CatelDependencyResolver(null));
            }
        }

        [TestClass]
        public class TheCanResolveMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullType()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => dependencyResolver.CanResolve(null));
            }

            [TestMethod]
            public void ReturnsFalseForNonRegisteredType()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                Assert.IsFalse(dependencyResolver.CanResolve(typeof(ITestInterface)));
            }

            [TestMethod]
            public void ReturnsTrueForRegisteredType()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                serviceLocator.RegisterType<IMessageService, MessageService>();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                Assert.IsTrue(dependencyResolver.CanResolve(typeof(IMessageService)));
            }
        }

        [TestClass]
        public class TheCanResolveAllMethod
        {
            [TestMethod]
            public void ThrowsArgumentExceptionForNullTypes()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => dependencyResolver.CanResolveAll(null));
            }

            [TestMethod]
            public void ReturnsTrueForEmptyArray()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                Assert.IsTrue(dependencyResolver.CanResolveAll(new Type[] { }));
            }

            [TestMethod]
            public void ReturnsFalseWhenNotAllTypesCanBeResolved()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                serviceLocator.RegisterType<IMessageService, MessageService>();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                var typesToResolve = new[] { typeof(ITestInterface), typeof(INavigationService), typeof(ITypeFactory) };

                Assert.IsFalse(dependencyResolver.CanResolveAll(typesToResolve));
            }

            [TestMethod]
            public void ReturnsTrueWhenAllTypesCanBeResolved()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                serviceLocator.RegisterType<IMessageService, MessageService>();
                serviceLocator.RegisterType<INavigationService, NavigationService>();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                var typesToResolve = new[] { typeof(IMessageService), typeof(INavigationService), typeof(ITypeFactory) };
                Assert.IsTrue(dependencyResolver.CanResolveAll(typesToResolve));
            }
        }

        [TestClass]
        public class TheResolveMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullType()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => dependencyResolver.Resolve(null));
            }

            [TestMethod]
            public void ThrowsTypeNotRegisteredForNonRegisteredType()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                ExceptionTester.CallMethodAndExpectException<TypeNotRegisteredException>(() => dependencyResolver.Resolve(typeof(ITestInterface)));
            }

            [TestMethod]
            public void ReturnsInstanceForRegisteredType()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                serviceLocator.RegisterType<IMessageService, MessageService>();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                Assert.IsNotNull(dependencyResolver.Resolve(typeof(IMessageService)));
            }
        }

        [TestClass]
        public class TheResolveAllMethod
        {
            [TestMethod]
            public void ThrowsArgumentExceptionForNullTypes()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => dependencyResolver.ResolveAll(null));
            }

            [TestMethod]
            public void ReturnsEmptyArrayForEmptyArray()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                var resolvedObjects = dependencyResolver.ResolveAll(new Type[] {});
                Assert.AreEqual(0, resolvedObjects.Length);
            }

            [TestMethod]
            public void ReturnsArrayWithNullValuesForNonRegisteredTypes()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                serviceLocator.RegisterType<IMessageService, MessageService>();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                var typesToResolve = new[] { typeof(IMessageService), typeof(ITestInterface), typeof(ITypeFactory) };
                var resolvedTypes = dependencyResolver.ResolveAll(typesToResolve);

                Assert.IsNotNull(resolvedTypes[0] as IMessageService);
                Assert.IsNull(resolvedTypes[1]);
                Assert.IsNotNull(resolvedTypes[2] as ITypeFactory);
            }

            [TestMethod]
            public void ReturnsArrayWithAllValuesForRegisteredTypes()
            {
                var serviceLocator = IoCFactory.CreateServiceLocator();
                serviceLocator.RegisterType<IMessageService, MessageService>();
                serviceLocator.RegisterType<INavigationService, NavigationService>();
                var dependencyResolver = serviceLocator.ResolveType<IDependencyResolver>();

                var typesToResolve = new[] { typeof(IMessageService), typeof(INavigationService), typeof(ITypeFactory) };
                var resolvedTypes = dependencyResolver.ResolveAll(typesToResolve);

                Assert.IsNotNull(resolvedTypes[0] as IMessageService);
                Assert.IsNotNull(resolvedTypes[1] as INavigationService);
                Assert.IsNotNull(resolvedTypes[2] as ITypeFactory);
            }
        }
    }
}