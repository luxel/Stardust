#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange;
using strange.extensions.mediation.impl;
using Stardust;
using Stardust.Pool;
using Stardust.Services;

public class ServiceManagerTestView : View {

	private IGameManager _GameManager;

	[Inject]
	public IGameManager GameManager 
	{
		get
		{
			return _GameManager;
		}
		set
		{
			_GameManager = value;
		}
	}

	[Inject]
	public IGameServiceManager ServiceManager
	{
		get; set;
	}

	public int TestLoopCount = 1000;

	protected override void Start ()
	{
		base.Start ();
		Invoke("Switch", 5f);
	}

	// Update is called once per frame
	void Update () {
		if (service == null)
		{
			Debug.LogError("Couldn't find service!");
			return;
		}
		TestFindServicePerformance();
	}

	#region Test Find service performance

	private bool useReflection = true;

	private bool findServiceWithType = true;

	private ILogService service;

	void Switch()
	{
		useReflection = false;
		Invoke("Switch2", 5f);
	}

	void Switch2()
	{
		findServiceWithType = false;
	}

	private void TestFindServicePerformance()
	{
		if (useReflection)
		{
			TestWithReflection();
		}
		else
		{
			if (findServiceWithType)
			{
				TestWithStardust();
			}
			else
			{
				TestWithStardust2();
			}
		}
	}

	/// <summary>
	/// Tests with GetService<T>() API
	/// </summary>
	private void TestWithStardust()
	{
		for (int i = 0; i < TestLoopCount; i ++)
		{
			service = ServiceManager.GetService<ILogService>();

		}
	}

	/// <summary>
	/// Tests with GetService<T>(name) API
	/// </summary>
	private void TestWithStardust2()
	{
		for (int i = 0; i < TestLoopCount; i ++)
		{
			service = ServiceManager.GetService<ILogService>("ILogService");
		}
	}

	/// <summary>
	/// Tests with strangeioc reflection
	/// </summary>
	private void TestWithReflection()
	{
		for (int i = 0; i < TestLoopCount; i ++)
		{
			service = GameManager.GetInstance<ILogService>();
		}
	}
	#endregion
}
#endif