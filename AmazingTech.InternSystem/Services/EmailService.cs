using System.Net.Mail;
using System.Net;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Data.Entity;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Mime;

namespace AmazingTech.InternSystem.Services
{
    public interface IEmailService
    {
        public void SendMail(string Content, string ReceiveAddress, String Subject, string InternId);
        public void SendMail2(string Content, string ReceiveAddress, String Subject);
        public void SendResultInterviewEmail(string Content, string ReceiveAddress, String Subject);
        public void SendRegistrationSuccessfulMail(User user);
    }

    public class EmailService : IEmailService
    {

        public EmailService()
        {

        }

        public void SendMail(string Content,string ReceiveAddress, String Subject, String InternId)
        {
            // var confirmLink = $"https://localhost:7078/api/lich-phong-vans/confirmEmail?id={InternId}";
            var confirmLink = $"https://internsystem.zouzoumanagement.xyz/api/lich-phong-vans/confirmEmail?id={InternId}";
            string contentConfirm = "<br> <br>Please click this link to confirm your interview: " + confirmLink + "<br>";
            string htmlCode = "<div dir=\"ltr\" class=\"gmail_signature\" data-smartmail=\"gmail_signature\"><div dir=\"ltr\"><div style=\"color:rgb(34,34,34)\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"background:transparent;font-size:14px;border-spacing:0px;border-collapse:collapse;font-family:Arial,sans-serif;color:rgb(46,154,164);max-width:500px\"><tbody><tr><td colspan=\"2\" width=\"500\" style=\"padding:0px 0px 4px;border-bottom:1px solid rgb(47,154,163)\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:18pt\"><span style=\"font-weight:700\"><font color=\"#0b5394\">--<br>Amazing Tech</font></span></span><br></font><span style=\"color:rgb(58,67,69);font-family:Tahoma,sans-serif;font-size:13.3333px;font-weight:700\">Software Development Company</span>&nbsp;&nbsp;<font face=\"tahoma, sans-serif\"><span style=\"font-size:10pt\">&nbsp;&nbsp;<font color=\"#6fa8dc\">&nbsp;</font><font color=\"#0b5394\">|</font>&nbsp;</span><a href=\"http://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(17,85,204);background-color:transparent;font-size:10pt;font-weight:bold\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=http://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw1vaYI2qEOK2T13SAzEBs2K\"><span style=\"font-size:10pt\"><font color=\"#0b5394\">amazingtech.vn</font></span></a></font></td></tr><tr><td width=\"120\" style=\"padding:22px 0px 0px\"><p style=\"margin:0px;padding:0px\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Photo\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NYIQZghSrU6IZdMz9suNlE9PjPuyZ1EdHCVmzj1vU2jZHotFhiSOIh7oGk9mHRBWejqWtG8D56PF6Q3q59IV4nwG1DVQ2MRcXQl_NltCnkIt-h6k3jG4aAtPnP9QkKcWec1tfc=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/logo-amazing-tech-v2.png\" style=\"border:0px;vertical-align:middle;max-width:90px\" class=\"CToWUd\" data-bit=\"iit\"></font></p></td><td style=\"padding:25px 0px 0px\"><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">T:</span>&nbsp;</font>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(028) 9999 - 7939</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">M:</font></span>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(+84) 888 181 100</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">E:</font></span><font color=\"#3d85c6\">&nbsp;</font><a href=\"mailto:amazingtech.hr@gmail.com\" style=\"color:rgb(16,16,16);background-color:transparent;font-size:9pt;line-height:10pt\" target=\"_blank\"><span style=\"font-size:9pt;line-height:10pt\">amazingtech.hr@gmail.com</span></a></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">A:</span>&nbsp;</font><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">74 Nguyễn Cửu Đàm, Phường Tân Sơn Nhì, Quận Tân Phú, TP.HCM</span></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\"><font face=\"tahoma, sans-serif\">&nbsp; &nbsp; &nbsp;S9.02A Vinhome Grand Park, Nguyễn Xiển, Phường Phước Thiện, Quận Thủ Đức, TP.HCM</font></span></p><p style=\"margin:4px 0px 0px;padding-bottom:0px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><a href=\"https://www.facebook.com/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0PsTtrffG8GO6TFt1mZkDe\"><img border=\"0\" width=\"20\" alt=\"facebook icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Nb-H8iXOIGiIIOjCq60PcYJdwAiL3JI9ca8Zvfz7PzPeRO7EP5yLfmQlXiaR8gk8shP-__vXefTiEmbGIHRo-lRY8PhPrnocnAIhlpItBw65ruLKdU7nElsW1pxywyN9YiYA_O7zIbih2al=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/fb.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.youtube.com/@amazingtechvietnam\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.youtube.com/@amazingtechvietnam&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0Zu2_6T3U_WB0AOHMaEeUq\"><img border=\"0\" width=\"20\" alt=\"youtube icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NaAgTDtnkTEwIcx8mxmO4-t06QPEl5sXLKF7gnOvFVQ8c31ca2W8BQmyAIlOBlSAjvOrXh_A452L86DfYtVK0dug2BbivN6r8XOyEEjnuvCDGtihKh_bNird28dMxannXIagpI_NkDRXrF4=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/yt.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.linkedin.com/company/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.linkedin.com/company/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw2Tb7NjZTwhK_taupiJr9iw\"><img border=\"0\" width=\"20\" alt=\"linkedin icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Na22lJt_btcmFPUMmTXijHqUbMnhZF2iNtgvomlgswJg-7vB888Zz5mYQcCEZUJa3yv8bugefIkEYkRQaxxeNYbBGXdytU5BAgd5UyfBRUSkr4Whm9q6UjET04sVtiekVBDhGzB-6L_JUwn=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/ln.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;</font></p></td></tr><tr><td colspan=\"2\" style=\"padding:25px 0px 0px\"><a href=\"https://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw11h_SNDADPSAg0WiKKnBHk\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Banner\" width=\"527\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NZT7FwFWtXD-LTbufKOqpFfhg4b1eEPlZOVdgqqEYnGh-4T6jGP9ZYxGLs3OM5QSw2rwPVXTRBWooBDxsi466XySJNe9bvDYVVSqpql6UIqKEITfpN3U68wh91TZUyh=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/icon-1-1024x761.png\" height=\"392\" style=\"border:0px;vertical-align:middle;margin-right:0px\" class=\"CToWUd\" data-bit=\"iit\"></font></a></td></tr></tbody></table></div></div></div>";
            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = Subject,
                    Body = Content + contentConfirm+htmlCode,
                    IsBodyHtml = true,
                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(ReceiveAddress);

                var smtp = new SmtpClient()
                {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void SendMail2(string Content, string ReceiveAddress, String Subject)
        {
            try
            {
                string htmlCode = "<div dir=\"ltr\" class=\"gmail_signature\" data-smartmail=\"gmail_signature\"><div dir=\"ltr\"><div style=\"color:rgb(34,34,34)\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"background:transparent;font-size:14px;border-spacing:0px;border-collapse:collapse;font-family:Arial,sans-serif;color:rgb(46,154,164);max-width:500px\"><tbody><tr><td colspan=\"2\" width=\"500\" style=\"padding:0px 0px 4px;border-bottom:1px solid rgb(47,154,163)\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:18pt\"><span style=\"font-weight:700\"><font color=\"#0b5394\">--<br>Amazing Tech</font></span></span><br></font><span style=\"color:rgb(58,67,69);font-family:Tahoma,sans-serif;font-size:13.3333px;font-weight:700\">Software Development Company</span>&nbsp;&nbsp;<font face=\"tahoma, sans-serif\"><span style=\"font-size:10pt\">&nbsp;&nbsp;<font color=\"#6fa8dc\">&nbsp;</font><font color=\"#0b5394\">|</font>&nbsp;</span><a href=\"http://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(17,85,204);background-color:transparent;font-size:10pt;font-weight:bold\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=http://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw1vaYI2qEOK2T13SAzEBs2K\"><span style=\"font-size:10pt\"><font color=\"#0b5394\">amazingtech.vn</font></span></a></font></td></tr><tr><td width=\"120\" style=\"padding:22px 0px 0px\"><p style=\"margin:0px;padding:0px\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Photo\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NYIQZghSrU6IZdMz9suNlE9PjPuyZ1EdHCVmzj1vU2jZHotFhiSOIh7oGk9mHRBWejqWtG8D56PF6Q3q59IV4nwG1DVQ2MRcXQl_NltCnkIt-h6k3jG4aAtPnP9QkKcWec1tfc=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/logo-amazing-tech-v2.png\" style=\"border:0px;vertical-align:middle;max-width:90px\" class=\"CToWUd\" data-bit=\"iit\"></font></p></td><td style=\"padding:25px 0px 0px\"><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">T:</span>&nbsp;</font>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(028) 9999 - 7939</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">M:</font></span>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(+84) 888 181 100</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">E:</font></span><font color=\"#3d85c6\">&nbsp;</font><a href=\"mailto:amazingtech.hr@gmail.com\" style=\"color:rgb(16,16,16);background-color:transparent;font-size:9pt;line-height:10pt\" target=\"_blank\"><span style=\"font-size:9pt;line-height:10pt\">amazingtech.hr@gmail.com</span></a></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">A:</span>&nbsp;</font><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">74 Nguyễn Cửu Đàm, Phường Tân Sơn Nhì, Quận Tân Phú, TP.HCM</span></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\"><font face=\"tahoma, sans-serif\">&nbsp; &nbsp; &nbsp;S9.02A Vinhome Grand Park, Nguyễn Xiển, Phường Phước Thiện, Quận Thủ Đức, TP.HCM</font></span></p><p style=\"margin:4px 0px 0px;padding-bottom:0px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><a href=\"https://www.facebook.com/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0PsTtrffG8GO6TFt1mZkDe\"><img border=\"0\" width=\"20\" alt=\"facebook icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Nb-H8iXOIGiIIOjCq60PcYJdwAiL3JI9ca8Zvfz7PzPeRO7EP5yLfmQlXiaR8gk8shP-__vXefTiEmbGIHRo-lRY8PhPrnocnAIhlpItBw65ruLKdU7nElsW1pxywyN9YiYA_O7zIbih2al=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/fb.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.youtube.com/@amazingtechvietnam\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.youtube.com/@amazingtechvietnam&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0Zu2_6T3U_WB0AOHMaEeUq\"><img border=\"0\" width=\"20\" alt=\"youtube icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NaAgTDtnkTEwIcx8mxmO4-t06QPEl5sXLKF7gnOvFVQ8c31ca2W8BQmyAIlOBlSAjvOrXh_A452L86DfYtVK0dug2BbivN6r8XOyEEjnuvCDGtihKh_bNird28dMxannXIagpI_NkDRXrF4=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/yt.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.linkedin.com/company/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.linkedin.com/company/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw2Tb7NjZTwhK_taupiJr9iw\"><img border=\"0\" width=\"20\" alt=\"linkedin icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Na22lJt_btcmFPUMmTXijHqUbMnhZF2iNtgvomlgswJg-7vB888Zz5mYQcCEZUJa3yv8bugefIkEYkRQaxxeNYbBGXdytU5BAgd5UyfBRUSkr4Whm9q6UjET04sVtiekVBDhGzB-6L_JUwn=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/ln.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;</font></p></td></tr><tr><td colspan=\"2\" style=\"padding:25px 0px 0px\"><a href=\"https://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw11h_SNDADPSAg0WiKKnBHk\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Banner\" width=\"527\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NZT7FwFWtXD-LTbufKOqpFfhg4b1eEPlZOVdgqqEYnGh-4T6jGP9ZYxGLs3OM5QSw2rwPVXTRBWooBDxsi466XySJNe9bvDYVVSqpql6UIqKEITfpN3U68wh91TZUyh=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/icon-1-1024x761.png\" height=\"392\" style=\"border:0px;vertical-align:middle;margin-right:0px\" class=\"CToWUd\" data-bit=\"iit\"></font></a></td></tr></tbody></table></div></div></div>";
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = Subject,
                    Body = Content + htmlCode,
                    IsBodyHtml = true

                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(ReceiveAddress);
                
                var smtp = new SmtpClient()
                {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SendResultInterviewEmail(string Content, string ReceiveAddress, String Subject)
        {
            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = Subject,
                    Body = Content,
                    IsBodyHtml = true,
                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(ReceiveAddress);

                var smtp = new SmtpClient()
                {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SendRegistrationSuccessfulMail(User user)
        {
            var subject = "Welcome to AmazingTech, " + user.HoVaTen + "!";
            var body = "";
            var receiver = user.Email;

            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(receiver);

                var smtp = new SmtpClient()
                {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
