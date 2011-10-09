﻿using System;

namespace Cloney.Core.Abstractions
{
    public interface ICanCloneSolution
    {
        event EventHandler CloningBegun;
        event EventHandler CloningEnded;

        String CurrentPath { get; }

        void CloneSolution(string sourcePath, string sourceNamespace, string targetPath, string targetNamespace);
    }
}