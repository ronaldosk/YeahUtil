﻿using System;
using System.Text;
using System.IO;
using System.Reflection;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity.Util.Introspection;

namespace CodeBuilder.TemplateEngine
{
    using Util;

    public class NVelocityEngineAdapter : ITemplateEngine
    {
        private static Logger logger = InternalTrace.GetLogger(typeof(NVelocityEngineAdapter));
        private VelocityEngine velocityEngine;

        public NVelocityEngineAdapter()
        {
            velocityEngine = new VelocityEngine();
        }

        public bool Run(TemplateData templateData)
        {
            VelocityContext context = new VelocityContext();
            context.Put("tdo", templateData);

            try
            {
                string loaderPath = Path.GetDirectoryName(templateData.TemplateFileName);
                string templateFile = Path.GetFileName(templateData.TemplateFileName);
                velocityEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, loaderPath);
                velocityEngine.Init();

                Encoding encoding = Encoding.GetEncoding(templateData.Encoding);
                if (templateData.Encoding == "UTF-8")
                {
                    encoding = new UTF8Encoding(false);
                }
                Template template = velocityEngine.GetTemplate(templateFile);
                using (StreamWriter StreamWriter = new StreamWriter(templateData.CodeFileName, false, encoding))
                {
                    template.Merge(context, StreamWriter);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("NVelocityAdapter:{0}", templateData.CodeFileName), ex);
                return false;
            }
        }
    }

    public class NVelocityDuck : IDuck
    {
        private readonly object _instance;
        private readonly Type _instanceType;
        private readonly Type[] _extensionTypes;
        private Introspector _introspector;

        public NVelocityDuck(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            _extensionTypes = new Type[] { typeof(StringExtension) };
            _instance = instance;
            _instanceType = _instance.GetType();
        }

        public Introspector Introspector
        {
            get
            {
                if (_introspector == null)
                    _introspector = RuntimeSingleton.Introspector;
                return _introspector;
            }
            set { _introspector = value; }
        }

        public object GetInvoke(string propName)
        {
            return null;
        }

        public void SetInvoke(string propName, object value)
        {
        }

        public object Invoke(string method, params object[] args)
        {
            if (string.IsNullOrEmpty(method)) return null;

            MethodInfo methodInfo = Introspector.GetMethod(_instanceType, method, args);
            if (methodInfo != null) { return methodInfo.Invoke(_instance, args); }

            object[] extensionArgs = new object[args.Length + 1];
            extensionArgs[0] = _instance;
            Array.Copy(args, 0, extensionArgs, 1, args.Length);
            foreach (Type extensionType in _extensionTypes)
            {
                methodInfo = Introspector.GetMethod(extensionType, method, extensionArgs);
                if (methodInfo != null) { return methodInfo.Invoke(null, extensionArgs); }
            }

            return null;
        }
    }
}
