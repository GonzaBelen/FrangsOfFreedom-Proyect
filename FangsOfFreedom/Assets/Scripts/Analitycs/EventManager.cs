using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnalyticsEvent = Unity.Services.Analytics.Event;

public class EventManager : MonoBehaviour
{
    public class DamagedEvent : AnalyticsEvent
    {
		public DamagedEvent() : base("Damaged")
		{
		}

		public string enemy { set { SetParameter("enemy", value); } }
		public int safeSlain { set { SetParameter("safeSlain", value); } }
		public int level { set { SetParameter("level", value); } }
    }
}
