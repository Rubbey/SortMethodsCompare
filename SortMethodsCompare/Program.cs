using System;
using System.Diagnostics;


namespace SortMethodsCompare
{
    class Program
    {
        static void InsertionSort(int[] t)
        {
            for (uint i = 1; i < t.Length; i++)
            {
                uint j = i; // elementy 0 .. i-1 są już posortowane
                int Buf = t[j]; // bierzemy i-ty (j-ty) element
                while ((j > 0) && (t[j - 1] > Buf))
                { // przesuwamy elementy
                    t[j] = t[j - 1];
                    j--;
                }
                t[j] = Buf; // i wpisujemy na docelowe miejsce
            }
        } /* InsertionSort() */
        static void SelectionSort(int[] t)
        {
            uint k;
            for (uint i = 0; i < (t.Length - 1); i++)
            {
                int Buf = t[i]; // bierzemy i-ty element
                k = i; // i jego indeks
                for (uint j = i + 1; j < t.Length; j++)
                    if (t[j] < Buf) // szukamy najmniejszego z prawej
                    {
                        k = j;
                        Buf = t[j];
                    }
                t[k] = t[i]; // zamieniamy i-ty z k-tym
                t[i] = Buf;
            }
        } /* SelectionSort() */
        static void Heapify(int[] t, uint left, uint right)
        { // procedura budowania/naprawiania kopca
            uint i = left,
            j = 2 * i + 1;
            int buf = t[i]; // ojciec
            while (j <= right) // przesiewamy do dna stogu
            {
                if (j < right) // wybieramy większego syna
                    if (t[j] < t[j + 1]) j++;
                if (buf >= t[j]) break;
                t[i] = t[j];
                i = j;
                j = 2 * i + 1; // przechodzimy do dzieci syna
            }
            t[i] = buf;
        } /* Heapify() */
        static void HeapSort(int[] t)
        {
            uint left = ((uint)t.Length / 2),
            right = (uint)t.Length - 1;
            while (left > 0) // budujemy kopiec idąc od połowy tablicy
            {
                left--;
                Heapify(t, left, right);
            }
            while (right > 0) // rozbieramy kopiec
            {
                int buf = t[left];
                t[left] = t[right];
                t[right] = buf; // największy element
                right--; // kopiec jest mniejszy
                Heapify(t, left, right); // ale trzeba go naprawić
            }
        } /* HeapSort() */
        static void CocktailSort(int[] t)
        {
            int Left = 1, Right = t.Length - 1, k = t.Length - 1;
            do
            {
                for (int j = Right; j >= Left; j--) // przesiewanie od dołu
                    if (t[j - 1] > t[j])
                    {
                        int Buf = t[j - 1]; t[j - 1] = t[j]; t[j] = Buf;
                        k = j; // zamiana elementów i zapamiętanie indeksu
                    }
                Left = k + 1; // zacieśnienie lewej granicy
                for (int j = Left; j <= Right; j++) // przesiewanie od góry
                    if (t[j - 1] > t[j])
                    {
                        int Buf = t[j - 1]; t[j - 1] = t[j]; t[j] = Buf;
                        k = j; // zamiana elementów i zapamiętanie indeksu
                    }
                Right = k - 1; // zacieśnienie prawej granicy
            }
            while (Left <= Right);
        } /* CocktailSort() */
        static void QuickSortRecurse(int[] t, int l, int p)
        {
            int i, j, x;
            i = l;
            j = p;
            x = t[(l + p) / 2]; // (pseudo)mediana 
            do
            {
                while (t[i] < x) i++; // przesuwamy indeksy z lewej
                while (x < t[j]) j--; // przesuwamy indeksy z prawej
                if (i <= j) // jeśli nie minęliśmy się indeksami (koniec kroku)
                { // zamieniamy elementy
                    int buf = t[i]; t[i] = t[j]; t[j] = buf;
                    i++; j--;
                }
            }
            while (i <= j);
            if (l < j) qsort(t, l, j); // sortujemy lewą część (jeśli jest)
            if (i < p) qsort(t, i, p); // sortujemy prawą część (jeśli jest)
        } /* qsort() */
        static void QuickSortIter(int[] t)
        {
            int i, j, l, p, sp;
            int[] stos_l = new int[t.Length],
                stos_p = new int[t.Length];                // przechowywanie żądań podziału
            sp = 0; stos_l[sp] = 0; stos_p[sp] = t.Length - 1;  // rozpoczynamy od całej tablicy
            do
            {
                l = stos_l[sp]; p = stos_p[sp]; sp--;               // pobieramy żądanie podziału 
                do
                {
                    int x; i = l; j = p; x = t[(l + p) / 2];                       // analogicznie do wersji rekurencyjnej
                    do
                    {
                        while (t[i] < x) i++;
                        while (x < t[j]) j--;
                        if (i <= j)
                        {
                            int buf = t[i]; t[i] = t[j]; t[j] = buf;
                            i++; j--;
                        }
                    }
                    while (i <= j);
                    if (i < p)
                    {
                        sp++;
                        stos_l[sp] = i;
                        stos_p[sp] = p;
                    } // ewentualnie dodajemy żądanie podziału
                    p = j;
                } while (l < p);
            } while (sp >= 0);                                      // dopóki stos żądań nie będzie pusty 
        } /* qsort() */

        static void Main(string[] args)
        {

            const int NIter = 10; // Liczba powtórzeń testu.

            // I
            // Tablica generowna losowo.
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int[] ArrayRandom = new int[200000];
            for (int j = 0; j < ArrayRandom.Length; j++) ArrayRandom[j] = rnd.Next(int.MaxValue);

            Console.WriteLine("\nInsertionSort [tablica generowana losowo]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayRandom, TestArray, u);
                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    InsertionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            Console.WriteLine("\nSelectionSort [tablica generowana losowo]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayRandom, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    SelectionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nCoctailSort [tablica generowana losowo]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayRandom, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    CocktailSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nHeapSort [tablica generowana losowo]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayRandom, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    HeapSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }



            // Tablica rosnąca.
            int[] ArrayIncreasing = new int[200000];
            for (int j = 0; j < ArrayIncreasing.Length; j++) ArrayIncreasing[j] = j;

            Console.WriteLine("\nInsertionSort [tablica generowana rosnąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayIncreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    InsertionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            Console.WriteLine("\nSelectionSort [tablica generowana rosnąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayIncreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    SelectionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nCoctailSort [tablica generowana rosnąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayIncreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    CocktailSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nHeapSort [tablica generowana rosnąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayIncreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    HeapSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }


            // Tablica malejąca
            int[] ArrayDecreasing = new int[200000];
            for (int j = ArrayDecreasing.Length - 1; j >= 0; j--) ArrayDecreasing[j] = j;

            Console.WriteLine("\nInsertionSort [tablica generowana malejąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayDecreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    InsertionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            Console.WriteLine("\nSelectionSort [tablica generowana malejąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayDecreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    SelectionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nCoctailSort [tablica generowana malejąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayDecreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    CocktailSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nHeapSort [tablica generowana malejąco]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayDecreasing, TestArray, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    HeapSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            // Tablica stała
            int[] ArrayConstant = new int[200000];
            Random RandomNumber = new Random(Guid.NewGuid().GetHashCode());
            int ConstantNumber = RandomNumber.Next(int.MaxValue);
            for (int j = 0; j < ArrayConstant.Length; j++) ArrayConstant[j] = ConstantNumber;

            Console.WriteLine("\nInsertionSort [tablica stała]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                for (int j = 0; j < TestArray.Length; j++) TestArray[j] = 30;

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    InsertionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            Console.WriteLine("\nSelectionSort [tablica stała (number = {0})]\nARRAY SIZE:\t TIME [ms]:", ConstantNumber);
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                for (int j = 0; j < TestArray.Length; j++) TestArray[j] = 30;

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    SelectionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nCoctailSort [tablica stała]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                for (int j = 0; j < TestArray.Length; j++) TestArray[j] = 30;

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    CocktailSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nHeapSort [tablica stała]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                for (int j = 0; j < TestArray.Length; j++) TestArray[j] = 30;

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    HeapSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }


            // Tablica V-kształtna
            int[] ArrayVShape = new int[200000];
            int Index = 0;
            for (int i = ArrayVShape.Length / 2; i > 0; i--) ArrayVShape[Index++] = i;
            for (int i = 0; i < ArrayVShape.Length / 2; i++) ArrayVShape[Index++] = i;


            Console.WriteLine("\nInsertionSort [tablica V-kształtna]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayVShape, ((ArrayVShape.Length - u) / 2), TestArray, 0, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    InsertionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            Console.WriteLine("\nSelectionSort [tablica V-kształtna]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayVShape, ((ArrayVShape.Length - u) / 2), TestArray, 0, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    SelectionSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nCoctailSort [tablica V-kształtna]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayVShape, ((ArrayVShape.Length - u) / 2), TestArray, 0, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    CocktailSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }
            Console.WriteLine("\nHeapSort [tablica V-kształtna]\nARRAY SIZE:\t TIME [ms]:");
            for (int u = 50000; u <= 200000; u += 10000)
            {
                int[] TestArray = new int[u];
                Array.Copy(ArrayVShape, ((ArrayVShape.Length - u) / 2), TestArray, 0, u);

                double ElapsedSeconds;
                long ElapsedTime = 0, MinTime = long.MaxValue, MaxTime = long.MinValue, IterationElapsedTime;
                for (int n = 0; n < (NIter + 1 + 1); ++n)  // odejmujemy wartości skrajne
                {
                    long StartingTime = Stopwatch.GetTimestamp();
                    HeapSort(TestArray);
                    long EndingTime = Stopwatch.GetTimestamp();
                    IterationElapsedTime = EndingTime - StartingTime;
                    ElapsedTime += IterationElapsedTime;
                    if (IterationElapsedTime < MinTime) MinTime = IterationElapsedTime;
                    if (IterationElapsedTime > MaxTime) MaxTime = IterationElapsedTime;
                }
                ElapsedTime -= (MinTime + MaxTime);
                ElapsedSeconds = ElapsedTime * (1000.0 / (NIter * Stopwatch.Frequency));
                Console.WriteLine("{0,-12}\t{1}", u, ElapsedSeconds.ToString("F4"));
            }

            Console.WriteLine("Uwaga! Nastąpi wyjście z programu, Skopiuj i zapisz wyniki!");
            Console.ReadLine();
            Console.ReadLine();

        }
    }
}
