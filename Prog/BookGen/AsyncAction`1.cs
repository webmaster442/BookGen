//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.GeneratorStepRunners;
using BookGen.GeneratorSteps;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;


namespace BookGen;

public delegate Task AsyncAction<in T>(T obj);
