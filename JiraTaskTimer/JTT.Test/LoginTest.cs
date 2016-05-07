using JiraTaskTimer.Client.Events;
using JiraTaskTimer.Client.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using JiraTaskTimer.Client.Models;
using JiraTaskTimer.Properties;

namespace JTT.Test
{
    [TestClass]
    public class LoginTest
    {
        private readonly IProgram program;

        public LoginTest()
        {
            program = Substitute.For<IProgram>();
        }


        [TestMethod]
        public void LoginIsSuccessful()
        {
            var user = new User(Settings.Default.username, Settings.Default.password, Settings.Default.jiraurl);
            program.Login(user, status =>
            {
                Assert.AreEqual(LoginStatus.Success, status);
            });
        }

        [TestMethod]
        public void LoginIsUnsuccessful()
        {
            var user = new User("", "", Settings.Default.jiraurl);
            program.Login(user, status =>
            {
                Assert.AreEqual(LoginStatus.Failed, status);
            });
        }
    }
}
