using MemberClaim.Models;

internal class UserValidationRequestModel
{
    private readonly CaseStudyContext context = new CaseStudyContext();

    public string Name { get; set; }
    public string Password { get; set; }

    public Member IsValidate(string name, string password)
    {
        Member member = null;
        if (context.Members == null)
        {
            return member;
        }

        member = (from x in context.Members where x.Name == name && x.Password == MemberClaim.Models.EncryptDecrypt.EncodePasswordToBase64(password) select x).SingleOrDefault();
        return member;
    }
}
