using Serilog;
using Serilog.Core;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiplosBancosDeDados.Core.Infra.CrossCutting
{
    public static class CommonLogger
    {
        private static Logger _Logger;

        static CommonLogger()
        {
            Inicialize();
        }

        public static void Inicialize()
        {
            if (_Logger == null)
            {
                _Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            }
        }

        private static Logger Get()
        {
            Inicialize();
            return _Logger;
        }

        public static void WriteSyncMethod(this string logMessage, MethodBase methodBase)
        {
            logMessage.WriteSyncMethod("", methodBase);
        }

        public static void WriteSyncMethod(this string logMessage, string optionalLogMessage, MethodBase methodBase)
        {
            logMessage.WriteSyncMethod(optionalLogMessage, (methodBase != null) ? ("Informativo no Método " + FindNameSyncMethod(methodBase) + ", na classe " + FindClassNameSyncMethod(methodBase) + ".") : "Informativo: ", methodBase);
        }

        private static void WriteSyncMethod(this string logMessage, string optionalLogMessage, string mensagemMetodo, MethodBase methodBase)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(mensagemMetodo);
                stringBuilder.AppendLine(" Mensagem: " + logMessage);
                if (!string.IsNullOrEmpty(optionalLogMessage))
                {
                    stringBuilder.AppendLine(" Mensagem opcional: " + optionalLogMessage);
                }

                stringBuilder.AppendLine($" Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}.");
                Get().Information(stringBuilder.ToString());
            }
            catch (Exception exception)
            {
                exception.WriteWarningSyncMethod(optionalLogMessage, methodBase);
                throw;
            }
        }

        public static async Task WriteAsyncMethod(this string logMessage, string optionalLogMessage, MethodBase methodBase)
        {
            string text = await FindNameAsyncMethod(methodBase);
            string text2 = await FindClassNameAsyncMethod(methodBase);
            logMessage.WriteSyncMethod(optionalLogMessage, "Informativo no Método " + text + ", na classe " + text2 + ".", methodBase);
            await Task.CompletedTask;
        }

        public static async Task WriteAsyncMethod(this string logMessage, MethodBase methodBase)
        {
            await logMessage.WriteAsyncMethod("", methodBase);
        }

        private static async Task<string> FindNameAsyncMethod(MethodBase methodBase)
        {
            return await Task.Run(new Func<string>(findAction));
            string findAction()
            {
                if (methodBase.DeclaringType != null && !string.IsNullOrEmpty(methodBase.DeclaringType!.Name))
                {
                    string text = methodBase.DeclaringType!.Name;
                    if (text.Contains("<", StringComparison.CurrentCulture))
                    {
                        text = text.Replace("<", "").Substring(0, text.Contains(">", StringComparison.CurrentCulture) ? (text.IndexOf(">") - 1) : 0);
                    }

                    return text;
                }

                return methodBase.Name;
            }
        }

        private static async Task<string> FindClassNameAsyncMethod(MethodBase methodBase)
        {
            return await Task.Run(new Func<string>(findAction));
            string findAction()
            {
                if (methodBase.DeclaringType != null)
                {
                    if (methodBase.DeclaringType!.ReflectedType != null && !string.IsNullOrEmpty(methodBase.DeclaringType!.ReflectedType!.Name))
                    {
                        return methodBase.DeclaringType!.ReflectedType!.Name;
                    }

                    return methodBase.DeclaringType!.Name;
                }

                return (methodBase.ReflectedType != null) ? methodBase.ReflectedType!.Name : "";
            }
        }

        private static string FindNameSyncMethod(MethodBase methodBase)
        {
            string text = methodBase.Name;
            if (text.Contains("<"))
            {
                text = text.Replace("<", "");
            }

            if (text.Contains(">"))
            {
                text = text.Substring(0, text.IndexOf(">"));
            }

            return text;
        }

        private static string FindClassNameSyncMethod(MethodBase methodBase)
        {
            if (methodBase != null && methodBase.ReflectedType != null && methodBase.ReflectedType!.DeclaringType != null && !string.IsNullOrEmpty(methodBase.ReflectedType!.DeclaringType!.Name))
            {
                return methodBase.ReflectedType!.DeclaringType!.Name;
            }

            if (methodBase.DeclaringType != null)
            {
                if (methodBase.DeclaringType!.ReflectedType != null && !string.IsNullOrEmpty(methodBase.DeclaringType!.ReflectedType!.Name))
                {
                    return methodBase.DeclaringType!.ReflectedType!.Name;
                }

                return methodBase.DeclaringType!.Name;
            }

            if (!(methodBase.ReflectedType != null))
            {
                return "";
            }

            return methodBase.ReflectedType!.Name;
        }

        public static void WriteSyncMethod(this Exception exception, MethodBase methodBase)
        {
            try
            {
                exception.WriteException("", methodBase, "Ocorreu um erro no Método " + methodBase.Name + ", na classe " + methodBase.ReflectedType!.Name + ".");
            }
            catch (Exception exception2)
            {
                exception2.WriteWarningSyncMethod(methodBase);
                throw;
            }
        }

        public static void WriteSyncMethod(this Exception exception, string optionalLogMessage, MethodBase methodBase)
        {
            try
            {
                exception.WriteException(optionalLogMessage, methodBase, "Ocorreu um erro no Método " + methodBase.Name + ", na classe " + methodBase.ReflectedType!.Name + ".");
            }
            catch (Exception exception2)
            {
                exception2.WriteWarningSyncMethod(methodBase);
                throw;
            }
        }

        public static void WriteWarningSyncMethod(this Exception exception, string optionalLogMessage, MethodBase methodBase)
        {
            string mensagemMetodo = ((methodBase != null) ? ("Ocorreu um warning no Método " + methodBase.Name + ", na classe " + methodBase.ReflectedType!.Name + ".") : "Ocorreu um erro.");
            exception.WriteWarningSyncMethod(optionalLogMessage, mensagemMetodo);
        }

        public static void WriteWarningSyncMethod(this Exception exception, MethodBase methodBase)
        {
            exception.WriteWarningSyncMethod("", methodBase);
        }

        private static void WriteWarningSyncMethod(this Exception exception, string optionalLogMessage, string mensagemMetodo)
        {
            Exception ex = exception.InnerException ?? exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            string message = ex.Message;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(mensagemMetodo);
            stringBuilder.AppendLine("Mensagem: " + message);
            stringBuilder.AppendLine("Detalhes: " + exception.StackTrace + Environment.NewLine + ".");
            if (!string.IsNullOrEmpty(optionalLogMessage))
            {
                stringBuilder.AppendLine("Mensagem opcional: " + optionalLogMessage);
            }

            stringBuilder.AppendLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}.");
            Get().Warning(stringBuilder.ToString());
        }

        public static async Task WriteAsyncMethod(this Exception exception, MethodBase methodBase)
        {
            await exception.WriteAsyncMethod("", methodBase);
        }

        public static async Task WriteAsyncMethod(this Exception exception, string optionalLogMessage, MethodBase methodBase)
        {
            try
            {
                string methodName = await FindNameAsyncMethod(methodBase);
                string text = await FindClassNameAsyncMethod(methodBase);
                exception.WriteException(optionalLogMessage, methodBase, "Ocorreu um erro no Método " + methodName + ", na classe " + text + ".");
                await Task.CompletedTask;
            }
            catch (Exception exception2)
            {
                exception2.WriteWarningSyncMethod(optionalLogMessage, methodBase);
                await Task.CompletedTask;
                throw;
            }
        }

        public static async Task WriteWarningAsyncMethod(this Exception exception, MethodBase methodBase)
        {
            await exception.WriteWarningAsyncMethod("", methodBase);
        }

        public static async Task WriteWarningAsyncMethod(this Exception exception, string optionalLogMessage, MethodBase methodBase)
        {
            string methodName = await FindNameAsyncMethod(methodBase);
            string text = await FindClassNameAsyncMethod(methodBase);
            string mensagemMetodo = "Ocorreu um warning no Método " + methodName + ", na classe " + text + ".";
            exception.WriteWarningSyncMethod(optionalLogMessage, mensagemMetodo);
            await Task.CompletedTask;
        }

        private static void WriteException(this Exception exception, string optionalLogMessage, MethodBase methodBase, string mensagemMetodo)
        {
            try
            {
                Exception ex = exception.InnerException ?? exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                string message = ex.Message;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(mensagemMetodo);
                stringBuilder.AppendLine("Mensagem: " + message);
                if (!string.IsNullOrEmpty(optionalLogMessage))
                {
                    stringBuilder.AppendLine("Mensagem opcional: " + optionalLogMessage);
                }

                stringBuilder.AppendLine("Detalhes: " + exception.StackTrace + ".");
                stringBuilder.AppendLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}.");
                Get().Error(stringBuilder.ToString());
            }
            catch (Exception exception2)
            {
                exception2.WriteWarningSyncMethod(optionalLogMessage, methodBase);
                throw;
            }
        }
    }
}
