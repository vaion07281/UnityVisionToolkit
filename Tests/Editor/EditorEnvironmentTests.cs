using NUnit.Framework;
using UnityEditor;
using UnityVisionToolkit.Runtime;
using UnityVisionToolkit.Editor;

namespace UnityVisionToolkit.Tests.Editor
{
    public class EditorEnvironmentTests
    {
        [Test]
        public void Editor_CanReferenceRuntimeAttributes()
        {
            // Simple sanity check to ensure Editor tests can compile and reference Runtime code
            var attribute = new ReadOnlyAttribute();
            Assert.IsNotNull(attribute);
        }
    }
}