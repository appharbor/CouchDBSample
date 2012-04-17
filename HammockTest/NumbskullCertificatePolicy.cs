using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace HammockTest
{
	public class NumskullCertificatePolicy : ICertificatePolicy
	{
		public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate cert, WebRequest request, int problem)
		{
			return true;
		}
	}
}
