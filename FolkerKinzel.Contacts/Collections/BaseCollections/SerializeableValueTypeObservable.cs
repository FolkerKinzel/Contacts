using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FK.ContactData.BaseCollections
{
    /// <summary>
    /// Eine ObservableCollection, die das
    /// Data-Binding darüber informieren kann, wenn einem ihrer Elemente eine neue Referenz zugewiesen wird oder
    /// wenn der Wert geändert wird (sofern der Vergleichsoperator für den Wertvergleich überladen ist). 
    /// Die Klasse ist serialisierbar.
    /// </summary>
    /// <typeparam name="T">Eine beliebige Klasse.</typeparam>
    [Serializable()]
    public class SerializeableValueTypeObservable<T> : IList<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// Eine List&lt;T>, die die Datenspeicherung im Hintergrund übernimmt.
        /// </summary>
        protected List<T> collection;


        /// <summary>
        /// Das Event wird ausgelöst, wenn ein Element hinzugefügt, entfernt, geändert oder verschoben wird
        /// oder wenn die gesamte Collection aktualisiert wird. 
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        # region Konstruktoren

        /// <summary>
        /// Initialisiert eine neue Instanz der SerializeableValueTypeObservable&lt;T>-Klasse. 
        /// </summary>
        public SerializeableValueTypeObservable()
        {
            collection = new List<T>();
        }


        /// <summary>
        /// Initialisiert eine neue, leere Instanz der SerializeableValueTypeObservable&lt;T>-Klasse 
        /// mit der angegebenen Anfangskapazität.
        /// </summary>
        /// <param name="capacity">Die Anfangskapazität.</param>
        public SerializeableValueTypeObservable(int capacity)
        {
            collection = new List<T>(capacity);
        }


        /// <summary>
        /// Initialisiert eine neue Instanz der SerializeableValueTypeObservable&lt;T>-Klasse, 
        /// die aus der angegebenen Auflistung kopierte Elemente enthält 
        /// und eine ausreichende Kapazität für die Anzahl der kopierten Elemente aufweist.
        /// </summary>
        /// <param name="input">Die Auflistung, deren Elemente in die neue SerializeableValueTypeObservable&lt;T> kopiert werden.</param>
        public SerializeableValueTypeObservable(IEnumerable<T> input)
        {
            collection = new List<T>(input);
        }
        # endregion

        /// <summary>
        /// Fügt am Ende der SerializeableValueTypeObservable&lt;T> ein Objekt hinzu. 
        /// </summary>
        /// <param name="item">Das Objekt, das hinzugefügt wird.</param>
        public virtual void Add(T item)
        {
            
                DoAdd(item);
        }

        private void DoAdd(T item)
        {
            collection.Add(item);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }


        /// <summary>
        /// Fügt die Elemente der angegebenen Auflistung am Ende von SerializeableValueTypeObservable&lt;T> hinzu.
        /// </summary>
        /// <param name="coll">Die Collection, deren Elemente hinzugefügt werden.</param>
        public virtual void AddRange(IEnumerable<T> coll)
        {
         
                DoAddRange(coll);
        }

        private void DoAddRange(IEnumerable<T> coll)
        {
            collection.AddRange(coll);
            
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Entfernt alle Elemente aus SerializeableValueTypeObservable&lt;T>.
        /// </summary>
        public virtual void Clear()
        {
                DoClear();
        }

        private void DoClear()
        {
           
            collection.Clear();
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Ermittelt, ob die SerializeableValueTypeObservable&lt;T> einen bestimmten Wert enthält.
        /// </summary>
        /// <param name="item">Das zu suchende Objekt.</param>
        /// <returns>True, wenn sich item in SerializeableValueTypeObservable&lt;T> befindet, andernfalls false.</returns>
        public virtual bool Contains(T item)
        {
            var result = collection.Contains(item);
            return result;
        }

        /// <summary>
        /// Kopiert die Elemente von SerializeableValueTypeObservable&lt;T> in ein Array, 
        /// beginnend bei einem bestimmten Array-Index.
        /// </summary>
        /// <param name="array">Das eindimensionale Array, das das Ziel 
        /// der aus der SerializeableValueTypeObservable&lt;T> kopierten Elemente ist. 
        /// Für das Array muss eine nullbasierte Indizierung verwendet werden. </param>
        /// <param name="arrayIndex">Der nullbasierte Index in array, an dem das Kopieren beginnt.</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Ruft die Anzahl der Elemente ab, die tatsächlich in der MyObservable&lt;T> enthalten sind.
        /// </summary>
        public virtual int Count
        {
            get
            {
               return collection.Count;
            }
        }


        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob SerializeableValueTypeObservable&lt;T> schreibgeschützt ist.
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Entfernt das erste Vorkommen eines bestimmten Objekts aus der SerializeableValueTypeObservable&lt;T>. 
        /// </summary>
        /// <param name="item">Das zu entfernende Objekt.</param>
        /// <returns>True, wenn das item erfolgreich entfernt wurde, andernfalls false.
        /// Diese Methode gibt auch dann false zurück, wenn das item nicht in der ursprünglichen
        /// SerializeableValueTypeObservable&lt;T> gefunden wurde.</returns>
        public virtual bool Remove(T item)
        {
                return DoRemove(item);
        }

        private bool DoRemove(T item)
        {
            var index = collection.IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            var result = collection.Remove(item);
            if (result && CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return result;
        }


        /// <summary>
        /// Gibt einen Enumerator zurück, der die Collection durchläuft.
        /// </summary>
        /// <returns>Ein List&lt;T>.Enumerator für die SerializeableValueTypeObservable&lt;T>.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Bestimmt den Index eines bestimmten Elements in der SerializeableValueTypeObservable&lt;T>. Gibt -1 
        /// zurück, wenn das Element nicht gefunden wurde.
        /// </summary>
        /// <param name="item">Das zu suchende Objekt.</param>
        /// <returns>Der Index von item, wenn das Element in der Liste gefunden wird, andernfalls -1.</returns>
        public virtual int IndexOf(T item)
        {
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Fügt am angegebenen Index ein Element in die SerializeableValueTypeObservable&lt;T> ein. Ist der
        /// Index größer als der letzte Index der Collection, wird das Element am Ende der Collection
        /// angefügt.
        /// </summary>
        /// <param name="index">Der Index, an dem eingefügt wird.</param>
        /// <param name="item">Das Element, das eingefügt wird.</param>
        public virtual void Insert(int index, T item)
        {
             DoInsert(index, item);
        }

        private void DoInsert(int index, T item)
        {
            if (index > collection.Count) index = collection.Count;
            collection.Insert(index, item);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }


        /// <summary>
        /// Entfernt das Element am angegebenen Index aus der SerializeableValueTypeObservable&lt;T>.
        /// </summary>
        /// <param name="index">Der Index, an dem das Element entfernt wird.</param>
        public virtual void RemoveAt(int index)
        {
    
                DoRemoveAt(index);
        }

        private void DoRemoveAt(int index)
        {
            if (collection.Count == 0 || collection.Count <= index)
            {
                return;
            }
            collection.RemoveAt(index);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Ruft das Element am angegebenen Index ab oder legt dieses fest.
        /// </summary>
        /// <param name="index">Der nullbasierte Index des Elements, 
        /// das abgerufen oder festgelegt werden soll.</param>
        public virtual T this[int index]
        {
            get
            {
                var result = collection[index];
                return result;
            }
            set
            {
                if (collection.Count == 0 || collection.Count <= index)
                {
                    return;
                }
                if (!collection[index].Equals(value))
                {
                    collection[index] = value;
                    if (CollectionChanged != null)
                        CollectionChanged(this,
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }


        /// <summary>
        /// Verschiebt das Element am angegebenen Index an eine neue Position in der Auflistung.
        /// </summary>
        /// <param name="oldIndex">Der nullbasierte Index, 
        /// der die Position des zu verschiebenden Elements angibt. Existiert dieser Index nicht,
        /// passiert nichts.</param>
        /// <param name="newIndex">Der nullbasierte Index, 
        /// der die neue Position des Elements angibt. Ist dieser Index größer als der letzte Index
        /// der Collection, wird das Element am Ende der Collection angefügt.</param>
        public virtual void Move(int oldIndex, int newIndex)
        {
                MoveItem(oldIndex, newIndex);
        }

        private void MoveItem(int oldIndex, int newIndex)
        {
            T item = this[oldIndex];
        
            if (collection.Count == 0 || collection.Count <= oldIndex)
            {
                return;
            }
            collection.RemoveAt(oldIndex);
            if (newIndex > collection.Count) newIndex = collection.Count;
            collection.Insert(newIndex, item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }


        /// <summary>
        /// Erstellt ein Array aus SerializeableValueTypeObservable&lt;T>. 
        /// </summary>
        /// <returns>Ein Array, das die Elemente aus der SerializeableValueTypeObservable&lt;T> enthält.</returns>
        public virtual T[] ToArray()
        {
            T[] arr = new T[this.Count];
            this.CopyTo(arr, 0);
            return arr;
        }

    }//class
}//ns
