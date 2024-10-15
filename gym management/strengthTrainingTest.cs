using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using gym_management;

namespace Gym_Management.Test
{
    [TestFixture]
    public class StrengthTrainingTests
    {
        private strength_training strengthTraining;

        [SetUp]
        public void Setup()
        {
            // Mocking the GetUserWeight method
            strengthTraining = new strength_training("test_user");
        }

        [Test]
        public void TestCalculateCalorieBurn()
        {
            // Test case 1: Low intensity, 30 minutes, Squats
            double result1 = strengthTraining.calculate_calorieBurn(30, "Low", "Squats", 70); 
            Assert.AreEqual(110.25, result1);

            // Test case 2: Moderate intensity, 45 minutes, Deadlifts
            double result2 = strengthTraining.calculate_calorieBurn(45, "Moderate", "Deadlifts", 70); 
            Assert.AreEqual(199.5, result2);

            // Test case 3: High intensity, 20 minutes, Bench Presses
            double result3 = strengthTraining.calculate_calorieBurn(20, "High", "Bench Presses", 70);
            Assert.AreEqual(102.66666666666666, result3);
        }

        [Test]
        public void TestGetMETValue()
        {
            // Test case 1: Squats
            double result1 = strengthTraining.GetMETValue("Squats");
            Assert.AreEqual(3.5, result1);

            // Test case 2: Deadlifts
            double result2 = strengthTraining.GetMETValue("Deadlifts");
            Assert.AreEqual(3.8, result2);

            // Test case 3: Unknown exercise type
            double result3 = strengthTraining.GetMETValue("Unknown");
            Assert.AreEqual(3.0, result3);
        }

        [Test]
        public void TestMETValue_Intensity()
        {
            // Test case 1: Low intensity
            double result1 = strengthTraining.METValue_Intensity(3.5, "Low");
            Assert.AreEqual(3.15, result1);

            // Test case 2: Moderate intensity
            double result2 = strengthTraining.METValue_Intensity(3.8, "Moderate");
            Assert.AreEqual(3.8, result2);

            // Test case 3: High intensity
            double result3 = strengthTraining.METValue_Intensity(4.0, "High");
            Assert.AreEqual(4.4, result3);
        }
    }
}
