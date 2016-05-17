using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCtutorial;
using MVCtutorial.Controllers;

namespace MVCtutorial.Tests.Controllers
{
    class AlarmControllerTest
    {


        [TestClass]
        public class HomeControllerTest
        {
            [TestMethod]
            public void Form()
            {
                // Arrange
                AlarmController controller = new AlarmController();

                // Act
                ViewResult result = controller.Form() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
            }
        }
    }
}
