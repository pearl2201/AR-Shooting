using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPeople
{
    void TakeDamage(int damage);
    void Moving();
    void Shooting();
    void Dying();
}


public interface IPoolObj
{
    void Reset();
}