using DotNetX.KeyValueStorage;
using FluentAssertions;
using System;
using Xunit;

namespace Abstractions.Tests
{
    public class ByteArrayEqualityComparerTests
    {
        [Fact]
        public void DefaultIsNotNull()
        {
            ByteArrayEqualityComparer.Default.Should().NotBeNull();
        }

        [Fact]
        public void TwoNullsAreEqual()
        {
            ByteArrayEqualityComparer.Default.Equals(null, null).Should().BeTrue();
        }

        [Fact]
        public void NullAndNonNullAreNotEqual()
        {
            ByteArrayEqualityComparer.Default.Equals(null, Array.Empty<byte>()).Should().BeFalse();
        }

        [Fact]
        public void NonNullAndNullAreNotEqual()
        {
            ByteArrayEqualityComparer.Default.Equals(Array.Empty<byte>(), null).Should().BeFalse();
        }

        [Fact]
        public void TwoEmptyArraysAreEquals()
        {
            ByteArrayEqualityComparer.Default.Equals(Array.Empty<byte>(), Array.Empty<byte>()).Should().BeTrue();
        }

        [Fact]
        public void TwoDistinctLengthArraysAreNotEqual()
        {
            ByteArrayEqualityComparer.Default.Equals(new byte[] { 1, 2, 3 }, new byte[] { 1, 2 }).Should().BeFalse();
        }

        [Fact]
        public void TwoSameLengthArraysWithDistinctContentAreNotEqual()
        {
            ByteArrayEqualityComparer.Default.Equals(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 4 }).Should().BeFalse();
        }

        [Fact]
        public void TwoSameLengthArraysWithSameContentAreEqual()
        {
            ByteArrayEqualityComparer.Default.Equals(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }).Should().BeTrue();
        }

        [Fact]
        public void GetHashCodeOnEmptyArrayShouldBe0()
        {
            ByteArrayEqualityComparer.Default.GetHashCode(Array.Empty<byte>()).Should().Be(0);
        }

        [Fact]
        public void GetHashCodeOnNonEmptyArrayShouldProbablyNotBe0()
        {
            ByteArrayEqualityComparer.Default.GetHashCode(new byte[] { 1, 2, 3 }).Should().Be(1440);
        }
    }
}
