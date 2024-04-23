using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILanguageChangeListerner 
{
    public void NotifyLanguageChange(int language);
}
