using be.belgium.eid;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.Helper
{
    public class EIDReader
    {

        public static EID LaadEID()
        {
            EID eidReader = null;
            try
            {
                
                BEID_ReaderSet ReaderSet;
                ReaderSet = BEID_ReaderSet.instance();

                BEID_ReaderContext Reader;
                Reader = ReaderSet.getReader();


                if (Reader.isCardPresent())
                {
                    if (Reader.getCardType() == BEID_CardType.BEID_CARDTYPE_EID
                        || Reader.getCardType() == BEID_CardType.BEID_CARDTYPE_FOREIGNER
                        || Reader.getCardType() == BEID_CardType.BEID_CARDTYPE_KIDS)
                    {

                        eidReader= Load_eid(Reader);
                    }
                    else
                    {
                        eidReader.Error="CARD TYPE UNKNOWN";
                    }
                }
                else
                {
                    eidReader.Error = "CARD NOT PRESENT";
                }
         
               

                BEID_ReaderSet.releaseSDK();

                return eidReader;
            }

            catch (BEID_Exception ex)
            {
                //BEID_ReaderSet.releaseSDK();
                return null;
            }
            catch (Exception ex)
            {
                //BEID_ReaderSet.releaseSDK();
                return null;
            }
        }
        private static EID Load_eid(BEID_ReaderContext Reader)
        {
            EID reader = new EID();


            BEID_EIDCard card;
            card = Reader.getEIDCard();
            if (card.isTestCard())
            {
                card.setAllowTestCard(true);
            }

            BEID_EId doc;
            doc = card.getID();

            reader.FirstName = doc.getFirstName();
            reader.LastName = doc.getSurname();
            reader.Gender = doc.getGender();
            reader.BirthDate = doc.getDateOfBirth();
            reader.BirthLocation = doc.getLocationOfBirth();
            reader.Nationality = doc.getNationality();
            reader.Street = doc.getStreet();
            reader.Zipcode = doc.getZipCode();
            reader.Country = doc.getCountry();

            reader.ChipNumber = doc.getChipNumber();
            reader.RegisterNumber = doc.getNationalNumber();

    
            BEID_Picture picture;
            picture = card.getPicture();
            reader.Image = picture.getData().GetBytes();

            return reader;
        }

        public static object ConvertTo(Type type, EID eid)
        {
            if (eid != null)
            {
                if (typeof(Customer) == type)
                {
                    return new Customer()
                    {
                        Address = eid.Street,
                        Name = eid.FirstName,
                        SurName= eid.LastName,
                        Picture = eid.Image,
                        RegisterNumber = eid.RegisterNumber,
                        Balance=10
                    };
                }

                if (typeof(Employee) == type)
                {
                    return new Employee()
                    {
                        Address = eid.Street,
                        Name = eid.FirstName,
                        SurName = eid.LastName,
                        Picture = eid.Image,
                        RegisterNumber = eid.RegisterNumber,
                        Email=eid.FirstName.Trim() +"."+ eid.LastName.Trim() + "@hotmail.com",
                        Phone="050"
                    };
                }

            }
            return null;

        }

    }
}
