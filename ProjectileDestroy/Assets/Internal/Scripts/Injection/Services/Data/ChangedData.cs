using System.Collections.Generic;
public class ChangedData
{
    public int moneyCount = 100000;
    public List<Weapon> purchasedWeapons = new List<Weapon>() { Weapon.ColtPython, Weapon.Glock };
    public bool showAd = true;
    public int currentLevel = 0;
    public Weapon equipmentWeapon = Weapon.Glock;
}
