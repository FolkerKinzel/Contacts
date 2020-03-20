using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;


namespace FK.ContactData.BaseCollections
{
    /// <summary>
    /// Eine ObservableCollection, die das Data-Binding darüber informieren kann, wenn sich eine Property eines ihrer
    /// Elemente ändert. Die Klasse ist serialisierbar.
    /// </summary>
    /// <typeparam name="T">Eine Klasse, die INotifyPropertyChanged implementiert.</typeparam>
    [Serializable()]
    public class SerializeableObservable<T> : IList<T>, INotifyCollectionChanged where T : INotifyPropertyChanged
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


        #region Konstruktoren

        /// <summary>
        /// Initialisiert eine neue Instanz der SerializeableObservable&lt;T>-Klasse. 
        /// </summary>
        public SerializeableObservable()
        {
            collection = new List<T>();
            NotifyElementChanges = true;
        }

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der SerializeableObservable&lt;T>-Klasse 
        /// mit der angegebenen Anfangskapazität.
        /// </summary>
        /// <param name="capacity">Die Anfangskapazität.</param>
        public SerializeableObservable(int capacity)
        {
            collection = new List<T>(capacity);
            NotifyElementChanges = true;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der SerializeableObservable&lt;T>-Klasse, 
        /// die aus der angegebenen Auflistung kopierte Elemente enthält 
        /// und eine ausreichende Kapazität für die Anzahl der kopierten Elemente aufweist.
        /// </summary>
        /// <param name="input">Die Auflistung, deren Elemente in die neue SerializeableObservable&lt;T> kopiert werden.</param>
        public SerializeableObservable(IEnumerable<T> input)
        {
            collection = new List<T>(input);
            NotifyElementChanges = true;
        }

        #endregion


        /// <summary>
        /// Fügt am Ende der SerializeableObservable&lt;T> ein Objekt hinzu. 
        /// </summary>
        /// <param name="item">Das Objekt, das hinzugefügt wird.</param>
        public virtual void Add(T item)
        {
            if (item != null)
            {
                item.PropertyChanged += new PropertyChangedEventHandler(SerializeableObservable_PropertyChanged);
            }
            collection.Add(item);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));    
        }



        /// <summary>
        /// Fügt die Elemente der angegebenen Auflistung am Ende von SerializeableObservable&lt;T> hinzu.
        /// </summary>
        /// <param name="coll">Die Collection, deren Elemente hinzugefügt werden.</param>
        public virtual void AddRange(IEnumerable<T> coll)
        {
            foreach (T item in coll)
            {
                if (item != null)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(SerializeableObservable_PropertyChanged);
                }
            }
            collection.AddRange(coll);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        }




        /// <summary>
        /// Fügt am angegebenen Index ein Element in die SerializeableObservable&lt;T> ein. 
        /// </summary>
        /// <param name="index">Der Index, an dem eingefügt wird.</param>
        /// <param name="item">Das Element, das eingefügt wird.</param>
        public virtual void Insert(int index, T item)
        {
            if (item != null)
            {
                item.PropertyChanged += new PropertyChangedEventHandler(SerializeableObservable_PropertyChanged);
            }
            if (index > collection.Count) index = collection.Count;
            collection.Insert(index, item);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }


        /// <summary>
        /// Entfernt das erste Vorkommen eines bestimmten Objekts aus der SerializeableObservable&lt;T>. 
        /// </summary>
        /// <param name="item">Das zu entfernende Objekt.</param>
        /// <returns>True, wenn das item erfolgreich entfernt wurde, andernfalls false.
        /// Diese Methode gibt auch dann false zurück, wenn das item nicht in der ursprünglichen
        /// SerializeableObservable&lt;T> gefunden wurde.</returns>
        public virtual bool Remove(T item)
        {
            var index = collection.IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            if (collection[index] != null)
                collection[index].PropertyChanged -= SerializeableObservable_PropertyChanged;
            var result = collection.Remove(item);
            if (result && CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return result;
        }

        /// <summary>
        /// Entfernt das Element am angegebenen Index aus der SerializeableObservable&lt;T>.
        /// </summary>
        /// <param name="index">Der Index, an dem das Element entfernt wird.</param>
        public virtual void RemoveAt(int index)
        {
            if (collection.Count == 0 || collection.Count <= index)
            {

                return;
            }
            if (collection[index] != null)
                collection[index].PropertyChanged -= SerializeableObservable_PropertyChanged;
            collection.RemoveAt(index);
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
        /// Entfernt alle Elemente aus SerializeableObservable&lt;T>.
        /// </summary>
        public virtual void Clear()
        {
            collection.Clear();
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Kopiert die Elemente von SerializeableObservable&lt;T> in ein Array, 
        /// beginnend bei einem bestimmten Array-Index.
        /// </summary>
        /// <param name="array">Das eindimensionale Array, das das Ziel 
        /// der aus der SerializeableObservable&lt;T> kopierten Elemente ist. 
        /// Für das Array muss eine nullbasierte Indizierung verwendet werden. </param>
        /// <param name="arrayIndex">Der nullbasierte Index in array, an dem das Kopieren beginnt.</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Erstellt ein Array aus SerializeableObservable&lt;T>. 
        /// </summary>
        /// <returns>Ein Array, das die Elemente aus der SerializeableObservable&lt;T> enthält.</returns>
        public virtual T[] ToArray()
        {
            T[] arr = new T[this.Count];
            this.CopyTo(arr, 0);
            return arr;
        }


        /// <summary>
        /// Ermittelt, ob die SerializeableObservable&lt;T> einen bestimmten Wert enthält.
        /// </summary>
        /// <param name="item">Das zu suchende Objekt.</param>
        /// <returns>True, wenn sich item in SerializeableObservable&lt;T> befindet, andernfalls false.</returns>
        public virtual bool Contains(T item)
        {
            return collection.Contains(item);
        }

        /// <summary>
        /// Bestimmt den Index eines bestimmten Elements in der SerializeableObservable&lt;T>.
        /// </summary>
        /// <param name="item">Das zu suchende Objekt.</param>
        /// <returns>Der Index von item, wenn das Element in der Liste gefunden wird, andernfalls -1.</returns>
        public virtual int IndexOf(T item)
        {
            return collection.IndexOf(item);
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
                    if (collection[index] != null)
                    {
                        collection[index].PropertyChanged += new PropertyChangedEventHandler(SerializeableObservable_PropertyChanged);
                    }
                    if (CollectionChanged != null)
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, collection[index], index));
                }
                
            }
        }


        /// <summary>
        /// Ruft die Anzahl der Elemente ab, die tatsächlich in der SerializeableObservable&lt;T> enthalten sind.
        /// </summary>
        public virtual int Count
        {
            get { return collection.Count; }
        }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob SerializeableObservable&lt;T> schreibgeschützt ist.
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Wenn true (Standardeinstellung), wird bei Änderungen an Properties der Elemente der 
        /// Collection das CollectionChanged-Event ausgelöst. Durch Eingabe von false lässt sich 
        /// das Abschalten.
        /// </summary>
        public virtual bool NotifyElementChanges { get; set; }

        #region GetEnumerator


        /// <summary>
        /// Gibt einen Enumerator zurück, der die Collection durchläuft.
        /// </summary>
        /// <returns>Ein List&lt;T>.Enumerator für die SerializeableObservable&lt;T>.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        # endregion

        # region PropertyChanged

        /// <summary>
        /// Event-Handler: Löst das CollectionChanged - Event aus, wenn auf einem der Elemente der Collection
        /// das PropertyChanged-Event ausgelöst wurde.
        /// </summary>
        /// <param name="sender">Das auslösende Objekt.</param>
        /// <param name="e">Ein PropertyChangedEventArgs, das die Daten des PropertyChanged-Ereignisses enthält.</param>
        protected virtual void SerializeableObservable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!NotifyElementChanges) return;

            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

    }//class
}//namespace


