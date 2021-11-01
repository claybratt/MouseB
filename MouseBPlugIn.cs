using Rhino.PlugIns;
using System;


namespace MouseB
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class MouseBPlugIn : PlugIn

    {
        readonly MouseHook.MouseHook mHook = new MouseHook.MouseHook();

        public MouseBPlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the MouseBPlugIn plug-in.</summary>
        public static MouseBPlugIn Instance
        {
            get; private set;
        }

        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;

        /// <summary>
        /// Called by Rhino when loading this plug-in.
        /// </summary>
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            void EventB(object sender, MouseHook.RawMouseEventArgs e) =>
                Rhino.RhinoApp.RunScript((e.MouseData & (1 << 16)) != 0 ? "_Zoom" : "_Zoom _Target", false);
                //Rhino.RhinoApp.WriteLine("{0}",(e.MouseData & (1 << 16)) != 0 ? "Mouse4" : "Mouse5");
            mHook.MouseAction += new EventHandler<MouseHook.RawMouseEventArgs>(EventB);
            mHook.Start();

            Rhino.RhinoApp.WriteLine("MouseB");
            return LoadReturnCode.Success;
        }
        protected override void OnShutdown()
        {
            mHook.Stop();
        }
    }
}