﻿/*
 * Copyright 2014 Google Inc. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using BenchmarkDotNet.Attributes;
using MyGame.Example;

namespace FlatBuffers.Benchmarks
{
    //[EtwProfiler] - needs elevated privileges
    [MemoryDiagnoser]
    public class FlatBufferBuilderBenchmark
    {
        private const int NumberOfRows = 1000;

        [Benchmark]
        public void BuildMonsters()
        {
            const string nestedMonsterName = "NestedMonsterName";
            const short nestedMonsterHp = 600;
            const short nestedMonsterMana = 1024;

            for (int i = 0; i < NumberOfRows; i++)
            {
                // Create nested buffer as a Monster type
                var fbb1 = new FlatBufferBuilder(16);
                var str1 = fbb1.CreateString(nestedMonsterName);
                Monster.StartMonster(fbb1);
                Monster.AddName(fbb1, str1);
                Monster.AddHp(fbb1, nestedMonsterHp);
                Monster.AddMana(fbb1, nestedMonsterMana);
                var monster1 = Monster.EndMonster(fbb1);
                Monster.FinishMonsterBuffer(fbb1, monster1);
                var fbb1Bytes = fbb1.SizedByteArray();
                fbb1 = null;

                // Create a Monster which has the first buffer as a nested buffer
                var fbb2 = new FlatBufferBuilder(16);
                var str2 = fbb2.CreateString("My Monster");
                var nestedBuffer = Monster.CreateTestnestedflatbufferVector(fbb2, fbb1Bytes);
                Monster.StartMonster(fbb2);
                Monster.AddName(fbb2, str2);
                Monster.AddHp(fbb2, 50);
                Monster.AddMana(fbb2, 32);
                Monster.AddTestnestedflatbuffer(fbb2, nestedBuffer);
                var monster = Monster.EndMonster(fbb2);
                Monster.FinishMonsterBuffer(fbb2, monster);
            }
        }
    }
}
