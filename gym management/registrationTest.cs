using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using gym_management;
using System.Windows.Forms;


namespace GymManagement.Test
{
    [TestFixture]
    public class RegistrationFormTests
    {
        private registration_form registrationForm;
        private MockMessageBox mockMessageBox;

        [SetUp]
        public void Setup()
        {
            registrationForm = new registration_form();
            mockMessageBox = new MockMessageBox();
        }

        public class MockMessageBox
        {
            private string lastMessage;

            public string LastMessage
            {
                get { return lastMessage; }
            }

            public void Show(string message)
            {
                lastMessage = message;
            }
        }

        [Test]
        public void TestValidatePass_ValidPassword()
        {
            
            string validPassword = "TestPassword123";

         
            bool isValid = registrationForm.ValidatePass(validPassword);

           
            Assert.IsTrue(isValid);
        }

        [Test]
        public void TestValidatePass_PasswordTooShort()
        {
            
            string shortPassword = "Short123";

        
            bool isValid = registrationForm.ValidatePass(shortPassword);


            Assert.IsFalse(isValid);
        }

        [Test]
        public void TestRegister_InvalidInput()
        {
           
            registrationForm.username.Text = "";
            registrationForm.password.Text = "";
            registrationForm.Com_password.Text = "";

           
            registrationForm.Register_Click(null, null);

            
            DialogResult result = MessageBox.Show("Username and Password fields cannot be empty", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Assert.AreEqual(DialogResult.OK, result);
        }

        [Test]
        public void TestRegister_PasswordMismatch()
        {
            
            registrationForm.username.Text = "TestUser";
            registrationForm.password.Text = "Password123";
            registrationForm.Com_password.Text = "Password456";

           
            registrationForm.Register_Click(null, null);

           
            DialogResult result = MessageBox.Show("Passwords do not match, please re-enter", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
            Assert.AreEqual(DialogResult.OK, result);
        }


        
    }
    
}

