﻿using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kingsun.Core.Log4net
{
    public class ExtLogManager
    {
        #region Static Member Variables

        /// <summary>
        /// The wrapper map to use to hold the <see cref="WebLogImpl"/> objects
        /// </summary>
        private static readonly WrapperMap s_wrapperMap = new WrapperMap(new WrapperCreationHandler(WrapperCreationHandler));

        #endregion

        #region Constructor

        /// <summary>
        /// Private constructor to prevent object creation
        /// </summary>
        private ExtLogManager() { }

        #endregion

        #region Type Specific Manager Methods

        /// <summary>
        /// Returns the named logger if it exists
        /// </summary>
        /// <remarks>
        /// <para>If the named logger exists (in the default hierarchy) then it
        /// returns a reference to the logger, otherwise it returns
        /// <c>null</c>.</para>
        /// </remarks>
        /// <param name="name">The fully qualified logger name to look for</param>
        /// <returns>The logger found, or null</returns>
        public static IExtLog Exists(string name)
        {
            return Exists(Assembly.GetCallingAssembly(), name);
        }

        /// <summary>
        /// Returns the named logger if it exists
        /// </summary>
        /// <remarks>
        /// <para>If the named logger exists (in the specified domain) then it
        /// returns a reference to the logger, otherwise it returns
        /// <c>null</c>.</para>
        /// </remarks>
        /// <param name="domain">the domain to lookup in</param>
        /// <param name="name">The fully qualified logger name to look for</param>
        /// <returns>The logger found, or null</returns>
        public static IExtLog Exists(string domain, string name)
        {
            return WrapLogger(LoggerManager.Exists(domain, name));
        }

        /// <summary>
        /// Returns the named logger if it exists
        /// </summary>
        /// <remarks>
        /// <para>If the named logger exists (in the specified assembly's domain) then it
        /// returns a reference to the logger, otherwise it returns
        /// <c>null</c>.</para>
        /// </remarks>
        /// <param name="assembly">the assembly to use to lookup the domain</param>
        /// <param name="name">The fully qualified logger name to look for</param>
        /// <returns>The logger found, or null</returns>
        public static IExtLog Exists(Assembly assembly, string name)
        {
            return WrapLogger(LoggerManager.Exists(assembly, name));
        }

        /// <summary>
        /// Returns all the currently defined loggers in the default domain.
        /// </summary>
        /// <remarks>
        /// <para>The root logger is <b>not</b> included in the returned array.</para>
        /// </remarks>
        /// <returns>All the defined loggers</returns>
        public static IExtLog[] GetCurrentLoggers()
        {
            return GetCurrentLoggers(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Returns all the currently defined loggers in the specified domain.
        /// </summary>
        /// <param name="domain">the domain to lookup in</param>
        /// <remarks>
        /// The root logger is <b>not</b> included in the returned array.
        /// </remarks>
        /// <returns>All the defined loggers</returns>
        public static IExtLog[] GetCurrentLoggers(string domain)
        {
            return WrapLoggers(LoggerManager.GetCurrentLoggers(domain));
        }

        /// <summary>
        /// Returns all the currently defined loggers in the specified assembly's domain.
        /// </summary>
        /// <param name="assembly">the assembly to use to lookup the domain</param>
        /// <remarks>
        /// The root logger is <b>not</b> included in the returned array.
        /// </remarks>
        /// <returns>All the defined loggers</returns>
        public static IExtLog[] GetCurrentLoggers(Assembly assembly)
        {
            return WrapLoggers(LoggerManager.GetCurrentLoggers(assembly));
        }

        /// <summary>
        /// Retrieve or create a named logger.
        /// </summary>
        /// <remarks>
        /// <para>Retrieve a logger named as the <paramref name="name"/>
        /// parameter. If the named logger already exists, then the
        /// existing instance will be returned. Otherwise, a new instance is
        /// created.</para>
        /// 
        /// <para>By default, loggers do not have a set level but inherit
        /// it from the hierarchy. This is one of the central features of
        /// log4net.</para>
        /// </remarks>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>the logger with the name specified</returns>
        public static IExtLog GetLogger(string name)
        {
            return GetLogger(Assembly.GetCallingAssembly(), name);
        }

        /// <summary>
        /// Retrieve or create a named logger.
        /// </summary>
        /// <remarks>
        /// <para>Retrieve a logger named as the <paramref name="name"/>
        /// parameter. If the named logger already exists, then the
        /// existing instance will be returned. Otherwise, a new instance is
        /// created.</para>
        /// 
        /// <para>By default, loggers do not have a set level but inherit
        /// it from the hierarchy. This is one of the central features of
        /// log4net.</para>
        /// </remarks>
        /// <param name="domain">the domain to lookup in</param>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>the logger with the name specified</returns>
        public static IExtLog GetLogger(string domain, string name)
        {
            return WrapLogger(LoggerManager.GetLogger(domain, name));
        }

        /// <summary>
        /// Retrieve or create a named logger.
        /// </summary>
        /// <remarks>
        /// <para>Retrieve a logger named as the <paramref name="name"/>
        /// parameter. If the named logger already exists, then the
        /// existing instance will be returned. Otherwise, a new instance is
        /// created.</para>
        /// 
        /// <para>By default, loggers do not have a set level but inherit
        /// it from the hierarchy. This is one of the central features of
        /// log4net.</para>
        /// </remarks>
        /// <param name="assembly">the assembly to use to lookup the domain</param>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>the logger with the name specified</returns>
        public static IExtLog GetLogger(Assembly assembly, string name)
        {
            return WrapLogger(LoggerManager.GetLogger(assembly, name));
        }

        /// <summary>
        /// Shorthand for <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        /// <remarks>
        /// Get the logger for the fully qualified name of the type specified.
        /// </remarks>
        /// <param name="type">The full name of <paramref name="type"/> will 
        /// be used as the name of the logger to retrieve.</param>
        /// <returns>the logger with the name specified</returns>
        public static IExtLog GetLogger(Type type)
        {
            return GetLogger(Assembly.GetCallingAssembly(), type.FullName);
        }

        /// <summary>
        /// Shorthand for <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        /// <remarks>
        /// Get the logger for the fully qualified name of the type specified.
        /// </remarks>
        /// <param name="domain">the domain to lookup in</param>
        /// <param name="type">The full name of <paramref name="type"/> will 
        /// be used as the name of the logger to retrieve.</param>
        /// <returns>the logger with the name specified</returns>
        public static IExtLog GetLogger(string domain, Type type)
        {
            return WrapLogger(LoggerManager.GetLogger(domain, type));
        }

        /// <summary>
        /// Shorthand for <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        /// <remarks>
        /// Get the logger for the fully qualified name of the type specified.
        /// </remarks>
        /// <param name="assembly">the assembly to use to lookup the domain</param>
        /// <param name="type">The full name of <paramref name="type"/> will 
        /// be used as the name of the logger to retrieve.</param>
        /// <returns>the logger with the name specified</returns>
        public static IExtLog GetLogger(Assembly assembly, Type type)
        {
            return WrapLogger(LoggerManager.GetLogger(assembly, type));
        }

        #endregion

        #region Extension Handlers

        /// <summary>
        /// Lookup the wrapper object for the logger specified
        /// </summary>
        /// <param name="logger">the logger to get the wrapper for</param>
        /// <returns>the wrapper for the logger specified</returns>
        private static IExtLog WrapLogger(ILogger logger)
        {
            return (IExtLog)s_wrapperMap.GetWrapper(logger);
        }

        /// <summary>
        /// Lookup the wrapper objects for the loggers specified
        /// </summary>
        /// <param name="loggers">the loggers to get the wrappers for</param>
        /// <returns>Lookup the wrapper objects for the loggers specified</returns>
        private static IExtLog[] WrapLoggers(ILogger[] loggers)
        {
            IExtLog[] results = new IExtLog[loggers.Length];
            for (int i = 0; i < loggers.Length; i++)
            {
                results[i] = WrapLogger(loggers[i]);
            }
            return results;
        }

        /// <summary>
        /// Method to create the <see cref="ILoggerWrapper"/> objects used by
        /// this manager.
        /// </summary>
        /// <param name="logger">The logger to wrap</param>
        /// <returns>The wrapper for the logger specified</returns>
        private static ILoggerWrapper WrapperCreationHandler(ILogger logger)
        {
            return new ExtLogImpl(logger);
        }

        #endregion
    }
}