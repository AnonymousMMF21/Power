using KeLi.Power.Converter.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Power.ConverterTests.Collections
{
    [TestClass]
    public partial class CollectionConverterTests
    {
        [TestMethod]
        public void ToAnyTypeTest()
        {
            var info = new Info(1);

            var result = info.ToAnyType<Info, Test>();

            Assert.AreEqual(result != null, true);
        }
    }
}