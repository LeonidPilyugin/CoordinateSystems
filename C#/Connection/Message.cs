using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;

// Файл содержит класс Message и перечисление MessageType

namespace Connection
{
    /// <summary>
    /// Перечисление MessageType содержит возможные типы сообщений.
    /// </summary>
    public enum MessageType { M1, M2, M3, Got, Fail };

    /// <summary>
    /// Класс Message описывает сообщение.
    /// </summary>
    /// 
    /// <remarks>
    /// Константы:<br/>
    /// 1) <see cref="LIGHT_SPEED"/><br/>
    /// <br/>
    /// Поля:<br/>
    /// 1) <see cref="type"/><br/>
    /// 2) <see cref="path"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Message(MessageType, LinkedList&lt;Connector&gt;)"/><br/>
    /// 2) <see cref="Message(MessageType, params Connector[])"/><br/>
    /// 3) <see cref="Message(Message)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Type"/><br/>
    /// 2) <see cref="Path"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="Time(Connector, Connector)"/><br/>
    /// 2) <see cref="Distance(Connector, Connector)"/><br/>
    /// 3) <see cref="Send(Connector)"/><br/>
    /// 4) <see cref="FindNext(Connector)"/><br/>
    /// 5) <see cref="FindPrevious(Connector)"/><br/>
    /// </remarks>
    public class Message
    {
        #region consts
        /// <summary>
        /// Скорость света. Измеряется в метрах в секунду.
        /// </summary>
        public const double LIGHT_SPEED = 299792458.0;
        #endregion

        #region type
        /// <summary>
        /// Тип сообщения.
        /// </summary>
        protected MessageType type;

        /// <summary>
        /// Путь, который должно пройти сообщения в виде:
        /// [отправитель], [посредник1], ..., [получатель].
        /// </summary>
        protected LinkedList<Connector> path;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает полям значения из параметров.
        /// </summary>
        /// 
        /// <param name="type"> Тип сообщения.</param>
        /// <param name="path"> Путь, который должно пройти сообщение. Не должен быть null.</param>
        public Message(MessageType type, LinkedList<Connector> path)
        {
            this.type = type;
            this.path = new LinkedList<Connector>(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"> Тип сообщения.</param>
        /// <param name="connectors"> Средств связи, через которые должно пройти сообщение.</param>
        public Message(MessageType message, params Connector[] connectors)
        {
            type = message;
            path = new LinkedList<Connector>();
            foreach(Connector connector in connectors)
            {
                path.AddLast(connector);
            }
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="message"> Копируемое сообщение. Не должно быть null.</param>
        public Message(Message message)
        {
            type = message.type;
            path = new LinkedList<Connector>(message.path);
        }
        #endregion

        #region properties
        /// <summary>
        /// Тип сообщения.
        /// </summary>
        public MessageType Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Путь, который должно пройти сообщение.
        /// </summary>
        public LinkedList<Connector> Path
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// Получатель.
        /// </summary>
        public Connector Last
        {
            get
            {
                return Path.Last.Value;
            }
        }

        /// <summary>
        /// Отправитель.
        /// </summary>
        public Connector First
        {
            get
            {
                return Path.First.Value;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Время задержки сигнала между двумя средствами связи в миллисекундах.
        /// </summary>
        /// 
        /// <param name="connector1"> Средство связи. Не должно быть null.</param>
        /// <param name="connector2"> Средство связи. Не должно быть null.</param>
        /// 
        /// <returns>
        /// Время задержки сигнала между двумя средствами связи в миллисекундах.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        static public int Time(Connector connector1, Connector connector2)
        {
            return (int)(Distance(connector1, connector2) / LIGHT_SPEED * 1000.0);
        }

        /// <summary>
        /// Расстояние между двумя средствами связи в метрах.
        /// </summary>
        /// 
        /// <param name="connector1"> Средство связи. Не должно быть null.</param>
        /// <param name="connector2"> Средство связи. Не должно быть null.</param>
        /// 
        /// <returns>
        /// Расстояние между двумя средствами связи в метрах.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        static public double Distance(Connector connector1, Connector connector2)
        {
            if(connector1 == null)
            {
                throw new ArgumentNullException("connector1 mustn't be null");
            }
            if (connector2 == null)
            {
                throw new ArgumentNullException("connector2 mustn't be null");
            }
            return (connector1.VectorFromRoot - connector2.VectorFromRoot).Length;
        }

        /// <summary>
        /// Запускает новый поток, который ждет время задержки и вызывает получателя обрабатывать сообщение.
        /// </summary>
        /// 
        /// <param name="sender"> Получатель. Не должен быть null.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public void Send(Connector sender)
        {
            if(sender == null)
            {
                throw new ArgumentNullException("Sender mustn't be null");
            }

            Task.Run(() =>
            {
                // wait
                System.Threading.Thread.Sleep(Time(sender, FindNext(sender)));
                // call receiver
                FindNext(sender).GetMessage(this);
            });
        }

        /// <summary>
        /// Возвращает следующее в списке средство связи после переданного в параметре или null, если таких нет.
        /// </summary>
        /// 
        /// <param name="connector"> Средство связи. Не должно быть null.</param>
        /// 
        /// <returns>
        /// Следующее в списке средство связи после переданного в параметре или null, если таких нет.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Connector FindNext(Connector connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException("Connector mustn't be null");
            }

            return Path.Find(connector).Next?.Value;
        }

        /// <summary>
        /// Возвращает предыдущее в списке средство связи после переданного в параметре или null, если таких нет.
        /// </summary>
        /// 
        /// <param name="connector"> Средство связи. Не должно быть null.</param>
        /// 
        /// <returns>
        /// Предыдущее в списке средство связи после переданного в параметре или null, если таких нет.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Connector FindPrevious(Connector connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException("Connector mustn't be null");
            }

            return Path.Find(connector).Previous?.Value;
        }
        #endregion
    }
}
