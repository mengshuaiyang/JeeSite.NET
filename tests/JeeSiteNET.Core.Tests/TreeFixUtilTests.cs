using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class TreeFixUtilTests
{
    [Fact]
    public void FixTreeData_SingleRoot_ShouldSetCorrectValues()
    {
        var nodes = new List<TreeFixNode>
        {
            new() { Id = "1", Name = "Root", ParentId = "" }
        };
        var result = TreeFixUtil.FixTreeData(nodes);
        result.Should().HaveCount(1);
        var root = result[0];
        root.ParentCodes.Should().Be("");
        root.TreeLevel.Should().Be(0);
        root.TreeSort.Should().Be(10);
        root.TreeNames.Should().Be("Root");
    }

    [Fact]
    public void FixTreeData_ParentChild_ShouldSetCorrectValues()
    {
        var nodes = new List<TreeFixNode>
        {
            new() { Id = "1", Name = "Root", ParentId = "" },
            new() { Id = "2", Name = "Child", ParentId = "1" }
        };
        var result = TreeFixUtil.FixTreeData(nodes);
        result.Should().HaveCount(2);
        var child = result.Single(n => n.Id == "2");
        child.ParentCodes.Should().Be("1");
        child.TreeLevel.Should().Be(1);
        child.TreeSort.Should().Be(20);
        child.TreeNames.Should().Be("Root, Child");
    }

    [Fact]
    public void FixTreeData_MultiLevel_ShouldSetLevels()
    {
        var nodes = new List<TreeFixNode>
        {
            new() { Id = "1", Name = "Root", ParentId = "" },
            new() { Id = "2", Name = "Child", ParentId = "1" },
            new() { Id = "3", Name = "Grandchild", ParentId = "2" }
        };
        var result = TreeFixUtil.FixTreeData(nodes);
        result.Single(n => n.Id == "1").TreeLevel.Should().Be(0);
        result.Single(n => n.Id == "2").TreeLevel.Should().Be(1);
        result.Single(n => n.Id == "3").TreeLevel.Should().Be(2);
        result.Single(n => n.Id == "3").TreeNames.Should().Be("Root, Child, Grandchild");
        result.Single(n => n.Id == "3").ParentCodes.Should().Be("1,2");
    }
}
