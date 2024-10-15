using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using gym_management;

namespace Gym_Management.Test
{
    [TestFixture]
    public class RopeJumpingTests
    {
        private RopeJumping ropeJumping;

        [SetUp]
        public void Setup()
        {
            ropeJumping = new RopeJumping("test_user");
        }

        [Test]
        public void TestCalculateCalorieBurn()
        {
            RopeJumping ropeJumping = new RopeJumping("test_user");

           
            double result1 = ropeJumping.calculate_calorieBurn(30, "Low", 100);
            Assert.AreEqual(0.15000000000000002, result1);

            
            double result2 = ropeJumping.calculate_calorieBurn(45, "Moderate", 150);
            Assert.AreEqual(0.30000000000000004, result2);

           
            double result3 = ropeJumping.calculate_calorieBurn(20, "High", 80);
            Assert.AreEqual(0.59999999999999998, result3);
        }

        [Test]
        public void TestEnergyPerJump()
        {
            RopeJumping ropeJumping = new RopeJumping("test_user");

            
            double result1 = ropeJumping.EnergyPerJump("Low");
            Assert.AreEqual(0.05, result1);

            
            double result2 = ropeJumping.EnergyPerJump("Moderate");
            Assert.AreEqual(0.1, result2);

            
            double result3 = ropeJumping.EnergyPerJump("High");
            Assert.AreEqual(0.15, result3);
        }
    }
}
