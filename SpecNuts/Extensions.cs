﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SpecNuts.Model;
using TechTalk.SpecFlow;

namespace SpecNuts
{
	public static class Extensions
	{
		public static TestResult GetResult(this IEnumerable<ReportItem> items)
		{
			return items.Select(x => x.Result).GetResult();
		}

		public static TestResult GetResult(this IEnumerable<TestResult> results)
		{
			var testResults = results as TestResult[] ?? results.ToArray();
			if (testResults.Any(x => x == TestResult.failed))
			{
				return TestResult.failed;
			}

			if (testResults.Any(x => x == TestResult.pending))
			{
				return TestResult.pending;
			}

			if (testResults.Any(x => x == TestResult.skipped))
			{
				return TestResult.skipped;
			}

			if (testResults.Any(x => x == TestResult.undefined))
			{
				return TestResult.undefined;
			}

			return TestResult.passed;
		}

		internal static IEnumerable<string> GetPendingSteps(this ScenarioContext scenarioContenxt)
		{
			return typeof (ScenarioContext)
				.GetProperty("PendingSteps", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(ScenarioContext.Current, null) as IEnumerable<string>
			       ?? new string[0];
		}

		internal static string ReplaceFirst(this string s, string find, string replace)
		{
			var first = s.IndexOf(find, StringComparison.Ordinal);
			return s.Substring(0, first) + replace + s.Substring(first + find.Length);
		}

		internal static string GetParamName(this MethodInfo method, int index)
		{
			var retVal = string.Empty;

			if (method != null && method.GetParameters().Length > index)
			{
				retVal = method.GetParameters()[index].Name;
			}


			return retVal;
		}

		internal static ExceptionInfo ToExceptionInfo(this Exception ex)
		{
			if (ex == null)
			{
				return null;
			}

			return new ExceptionInfo(ex);
		}
	}
}