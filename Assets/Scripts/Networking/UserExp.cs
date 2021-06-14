[System.Serializable]
public class UserExp{


    public string userid = "123654";
    public string name = "testing" ;
    public string college = "testing" ;
    public string branch = "testing";
    public Values values;


    public void SetValues(float l,float d,float t1,float a1,float t2,float a2,float mg){
        RodInfo rodInfo = new RodInfo(l,d);
        RowInTable row01 = new RowInTable(t1,a1);
        RowInTable row02 = new RowInTable(t2,a2);
        ExpValues expValues = new ExpValues(row01,row02,mg);
        this.values = new Values(rodInfo,expValues);
    }

}


 [System.Serializable]
public class RodInfo{
    public float length;
    public float diameter;
    public RodInfo(float l,float d){
        this.length = l;
        this.diameter = d;
    }
}

[System.Serializable]
public class RowInTable{
    public float torque;
    public float angleOfTwist;
    public RowInTable(float t,float a){
        this.torque = t;
        this.angleOfTwist = a;
    }
}

[System.Serializable]
public class ExpValues{
    public RowInTable row01;
    public RowInTable row02;
    public float modulusG;
    public ExpValues(RowInTable r1,RowInTable r2,float g){
        this.row01 = r1;
        this.row02 = r2;
        this.modulusG = g;
    }
}

[System.Serializable]
public class Values{
    public RodInfo rodInfo;
    public ExpValues expValues;
    public Values(RodInfo ri,ExpValues ev){
        this.rodInfo = ri;
        this.expValues = ev; 
    }
}