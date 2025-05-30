﻿using Jacobi.Vst.Core.Host;
using Jacobi.Vst.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openthesia.Core.Plugins;

public class HostCommandStub : IVstHostCommandStub
{
    public HostCommandStub()
    {
        Commands = new HostCommands(this);
    }

    public event EventHandler<PluginCalledEventArgs> PluginCalled;

    private void RaisePluginCalled(string message)
    {
#if DEBUG
        PluginCalled?.Invoke(this, new PluginCalledEventArgs(message));
#endif
    }

    /// <summary>
    /// Attached to the EditorFrame for a plugin.
    /// </summary>
    public event EventHandler<SizeWindowEventArgs> SizeWindow;

    private void RaiseSizeWindow(int width, int height)
    {
#if DEBUG
        SizeWindow?.Invoke(this, new SizeWindowEventArgs(width, height));
#endif
    }

    public IVstPluginContext PluginContext { get; set; }
    public IVstHostCommands20 Commands { get; private set; }

    private sealed class HostCommands : IVstHostCommands20
    {
        private readonly HostCommandStub _cmdStub;

        public HostCommands(HostCommandStub cmdStub)
        {
            _cmdStub = cmdStub;
        }

        #region IVstHostCommands20 Members

        /// <inheritdoc />
        public bool BeginEdit(int index)
        {
            _cmdStub.RaisePluginCalled("BeginEdit(" + index + ")");
            return false;
        }

        /// <inheritdoc />
        public VstCanDoResult CanDo(string cando)
        {
            _cmdStub.RaisePluginCalled("CanDo(" + cando + ")");
            return VstCanDoResult.Unknown;
        }

        /// <inheritdoc />
        public bool CloseFileSelector(VstFileSelect fileSelect)
        {
            _cmdStub.RaisePluginCalled("CloseFileSelector(" + fileSelect.Command + ")");
            return false;
        }

        /// <inheritdoc />
        public bool EndEdit(int index)
        {
            _cmdStub.RaisePluginCalled("EndEdit(" + index + ")");
            return false;
        }

        /// <inheritdoc />
        public VstAutomationStates GetAutomationState()
        {
            _cmdStub.RaisePluginCalled("GetAutomationState()");
            return VstAutomationStates.Off;
        }

        /// <inheritdoc />
        public int GetBlockSize()
        {
            _cmdStub.RaisePluginCalled("GetBlockSize()");
            return 4096;
        }

        /// <inheritdoc />
        public string GetDirectory()
        {
            _cmdStub.RaisePluginCalled("GetDirectory()");
            return null;
        }

        /// <inheritdoc />
        public int GetInputLatency()
        {
            _cmdStub.RaisePluginCalled("GetInputLatency()");
            return 0;
        }

        /// <inheritdoc />
        public VstHostLanguage GetLanguage()
        {
            _cmdStub.RaisePluginCalled("GetLanguage()");
            return VstHostLanguage.NotSupported;
        }

        /// <inheritdoc />
        public int GetOutputLatency()
        {
            _cmdStub.RaisePluginCalled("GetOutputLatency()");
            return 0;
        }

        /// <inheritdoc />
        public VstProcessLevels GetProcessLevel()
        {
            _cmdStub.RaisePluginCalled("GetProcessLevel()");
            return VstProcessLevels.Unknown;
        }

        /// <inheritdoc />
        public string GetProductString()
        {
            _cmdStub.RaisePluginCalled("GetProductString()");
            return "VST.NET";
        }

        /// <inheritdoc />
        public float GetSampleRate()
        {
            _cmdStub.RaisePluginCalled("GetSampleRate()");
            return 44.1f;
        }

        /// <inheritdoc />
        public VstTimeInfo GetTimeInfo(VstTimeInfoFlags filterFlags)
        {
            //_cmdStub.RaisePluginCalled("GetTimeInfo(" + filterFlags + ")");
            return null;
        }

        /// <inheritdoc />
        public string GetVendorString()
        {
            _cmdStub.RaisePluginCalled("GetVendorString()");
            return "Jacobi Software";
        }

        /// <inheritdoc />
        public int GetVendorVersion()
        {
            _cmdStub.RaisePluginCalled("GetVendorVersion()");
            return 1000;
        }

        /// <inheritdoc />
        public bool IoChanged()
        {
            _cmdStub.RaisePluginCalled("IoChanged()");
            return false;
        }

        /// <inheritdoc />
        public bool OpenFileSelector(VstFileSelect fileSelect)
        {
            _cmdStub.RaisePluginCalled("OpenFileSelector(" + fileSelect.Command + ")");
            return false;
        }

        /// <inheritdoc />
        public bool ProcessEvents(VstEvent[] events)
        {
            _cmdStub.RaisePluginCalled("ProcessEvents(" + events.Length + ")");
            return false;
        }

        /// <inheritdoc />
        public bool SizeWindow(int width, int height)
        {
            _cmdStub.RaisePluginCalled("SizeWindow(" + width + ", " + height + ")");
            _cmdStub.RaiseSizeWindow(width, height);
            return false;
        }

        /// <inheritdoc />
        public bool UpdateDisplay()
        {
            _cmdStub.RaisePluginCalled("UpdateDisplay()");
            return false;
        }

        #endregion

        #region IVstHostCommands10 Members

        /// <inheritdoc />
        public int GetCurrentPluginID()
        {
            _cmdStub.RaisePluginCalled("GetCurrentPluginID()");
            // this is the plugin Id the host wants to load
            // for shell plugins (a plugin that hosts other plugins)
            return 0;
        }

        /// <inheritdoc />
        public int GetVersion()
        {
            _cmdStub.RaisePluginCalled("GetVersion()");
            return 1000;
        }

        /// <inheritdoc />
        public void ProcessIdle()
        {
            _cmdStub.RaisePluginCalled("ProcessIdle()");
        }

        /// <inheritdoc />
        public void SetParameterAutomated(int index, float value)
        {
            _cmdStub.RaisePluginCalled("SetParameterAutomated(" + index + ", " + value + ")");
            //_cmdStub.PluginContext.PluginCommandStub.Commands.EditorIdle()
        }

        #endregion
    }
}

public sealed class PluginCalledEventArgs : EventArgs
{
    /// <summary>
    /// Constructs a new instance with a <paramref name="message"/>.
    /// </summary>
    /// <param name="message"></param>
    public PluginCalledEventArgs(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; private set; }
}

/// <summary>
/// Event arguments used when the SizeWindow method is called.
/// </summary>
public sealed class SizeWindowEventArgs : EventArgs
{
    /// <summary>
    /// Constructs a new instance with a <paramref name="message"/>.
    /// </summary>
    /// <param name="message"></param>
    public SizeWindowEventArgs(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }
    public int Height { get; }
}
