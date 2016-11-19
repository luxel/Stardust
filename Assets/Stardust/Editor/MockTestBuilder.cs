#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using System.Collections;

    public class MockTestBuilder : PlayerBuilderWithEncryption
    {
        protected override void DoBuild()
        {
            // Do nothing.
            Log("Actual build is skipped for testing.");
        }
    }
}
#endif