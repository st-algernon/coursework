namespace Coursework_server.Pagination
{
    public class UsersPageParams
    {
        const int MaxSize = 50;
        private int _size = 15;
        public int Page { get; set; } = 1;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = (value > MaxSize) ? MaxSize : value;
            }
        }
    }
}
