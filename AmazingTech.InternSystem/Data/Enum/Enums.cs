using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Data.Enum
{
  
    public class Enums
    {
       public enum IdDuAn
        {
            InternSystem,
            HumanResource,
            ThepMienNam
        }

        public enum IdNhomZalo
        {
            InternT1_2024,
            HRMT1_2024,
            ThepMN
        }

        public enum IdViTri
        {
            BackEnd,
            FrontEnd,
            Tester,
            BA
        }

        public enum IdTruong
        {
            FPT,
            KhoaHocTuNhien,
            CNTT
        }
    }

    public enum Status
    {
        Not_Yet,
        Done
    }
    public enum Result
    {
        Failed,
        Pass
    }
    public enum InterviewForm
    {
        Online,
        Offline
    }

    public enum Rank
    {
        Intern,
        Senior,
        Junior
    }
    public enum TrangThaiThucTap 
    {
       Waiting,
       Practicing,        
       Failed,
       Finished
    }
}
