using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Power.DriveTests.Excel
{
    [TestClass]
    public class ExcelParameterTests
    {
        [TestMethod]
        public void ExcelParamTest()
        {
            var param = new ExcelParameter(@"Resources\Instance.xlsx", @"Resources\Template.xlsx");

            Assert.AreEqual(param.FilePath.Length, param.TemplatePath.Length);
        }
    }
}