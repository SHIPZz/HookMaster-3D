using System;

[Serializable]
public class PotentialEmployeeData 
{
    public Guid Guid;
    public string Name;
    public int QualificationType;
    public int Salary;
    public int Profit;

    public bool Equals(PotentialEmployeeData other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && QualificationType == other.QualificationType && Salary == other.Salary && Profit == other.Profit;
    }
    
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PotentialEmployeeData)obj);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, QualificationType, Salary, Profit);
    }
}
