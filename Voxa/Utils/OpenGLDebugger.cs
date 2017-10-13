using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Voxa.Utils
{
    public static class OpenGLDebugger
    {
        public static DebugProc DebugCallback = HandleDebugGLMessage;

        public static void HandleDebugGLMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {

            StringBuilder errorString = new StringBuilder();

            switch (source) {
                case DebugSource.DebugSourceApi: {
                    errorString.Append("[API]");
                    break;
                }
                case DebugSource.DebugSourceApplication: {
                    errorString.Append("[APP]");
                    break;
                }
                case DebugSource.DebugSourceOther: {
                    errorString.Append("[Other]");
                    break;
                }
                case DebugSource.DebugSourceShaderCompiler: {
                    errorString.Append("[Shader]");
                    break;
                }
                case DebugSource.DebugSourceThirdParty: {
                    errorString.Append("[3RD]");
                    break;
                }
                case DebugSource.DebugSourceWindowSystem: {
                    errorString.Append("[Window]");
                    break;
                }
            }

            switch (type) {
                case DebugType.DebugTypeDeprecatedBehavior: {
                    errorString.Append("[DEPRECATED]");
                    break;
                }
                case DebugType.DebugTypeError: {
                    errorString.Append("[ERROR]");
                    break;
                }
                case DebugType.DebugTypeMarker: {
                    errorString.Append("[MARKER]");
                    break;
                }
                case DebugType.DebugTypeOther: {
                    errorString.Append("[OTHER]");
                    break;
                }
                case DebugType.DebugTypePerformance: {
                    errorString.Append("[PERF]");
                    break;
                }
                case DebugType.DebugTypePopGroup: {
                    errorString.Append("[POPGROUP]");
                    break;
                }
                case DebugType.DebugTypePortability: {
                    errorString.Append("[PORTABILITY]");
                    break;
                }
                case DebugType.DebugTypePushGroup: {
                    errorString.Append("[PUSHGROUP]");
                    break;
                }
                case DebugType.DebugTypeUndefinedBehavior: {
                    errorString.Append("[BEHAVIOR]");
                    break;
                }
            }

            errorString.Append(Marshal.PtrToStringAnsi(message));

            switch (severity) {
                case DebugSeverity.DebugSeverityHigh: {
                    Logger.Error(errorString.ToString());
                    if (type == DebugType.DebugTypeError) throw new InvalidOperationException("ARB_debug_output found an error.");
                    break;
                }
                case DebugSeverity.DebugSeverityMedium: {
                    Logger.Warning(errorString.ToString());
                    break;
                }
                case DebugSeverity.DebugSeverityLow: {
                    errorString.Insert(0, "[Low]");
                    Logger.Info(errorString.ToString());
                    break;
                }
                case DebugSeverity.DebugSeverityNotification: {
                    errorString.Insert(0, "[Notice]");
                    Logger.Info(errorString.ToString());
                    break;
                }
                default: {
                    break;
                }

            }
        }
    }
}
