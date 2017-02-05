﻿using System;
using System.Runtime.Serialization;
using Tomataboard.Services.Photos.Api500px.Intefaces;

namespace Tomataboard.Services.Photos.Api500px.Models
{
    [DataContract]
    public class UserDetails : User, IUserDetails
    {
        #region Public Properties

        [DataMember(Name = "about")]
        public string About { get; set; }

        [DataMember(Name = "domain")]
        public string Domain { get; set; }

        [DataMember(Name = "friends_count")]
        public long FriendsCount { get; set; }

        [DataMember(Name = "locale")]
        public string Locale { get; set; }

        [DataMember(Name = "photos_count")]
        public long PhotosCount { get; set; }

        [DataMember(Name = "registration_date")]
        public DateTime RegistrationDate { get; set; }

        [DataMember(Name = "sex")]
        public int? Sex { get; set; }

        [DataMember(Name = "show_nude")]
        public bool ShowNude { get; set; }

        [DataMember(Name = "State")]
        public string State { get; set; }

        [DataMember(Name = "store_on")]
        public bool StoreEnabled { get; set; }

        #endregion Public Properties
    }
}