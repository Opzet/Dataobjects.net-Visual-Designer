using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public class VersionNumber : IComparable, ICloneable
    {
        #region enum VersionComponent

        public enum VersionComponent
        {
            Major,
            Minor,
            Build,
            Revision
        }

        #endregion enum VersionComponent

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <c>ProjectVersion</c> class 
        ///   with a non-defined (empty) version.
        /// </summary>
        private VersionNumber()
        {
            m_version = new ListDictionary();
            Valid = true;
        }

        private VersionNumber(bool valid)
            : this()
        {
            Valid = valid;
        }

        /// <summary>
        ///   Initializes a new instance of the <c>ProjectVersion</c> class using 
        ///   the value represented by the specified <c>String</c>.
        /// </summary>
        /// <param name="version">
        ///   Version string of the form: [Major].[Minor].[Build].[Revision]. 
        ///   Major and Minor components are obligatory, while Build and 
        ///   Revison may be omitted or substituted by a single or two 
        ///   asterisks.
        /// </param>
        public VersionNumber(string version)
            : this()
        {
            version = version.Replace(",", ".");

            m_originalString = version;
            SplitComponents(version);
        }

        /// <summary>
        ///   Initializes a new instance of the <c>ProjectVersion</c> class 
        ///   with the specified major, minor, build, and revision numbers.
        /// </summary>
        /// <param name="major">
        ///   The major version number.
        /// </param>
        /// <param name="minor">
        ///   The minor version number.
        /// </param>
        /// <param name="build">
        ///   The build number.
        /// </param>
        /// <param name="revision">
        ///   The revision number.
        /// </param>
        private VersionNumber(int major, int minor, int build, int revision)
            : this(major, minor, build)
        {
            m_version[VersionComponent.Revision] = revision;
        }

        /// <summary>
        ///   Initializes a new instance of the <c>ProjectVersion</c> class 
        ///   with the specified major, minor and build numbers.
        /// </summary>
        /// <param name="major">
        ///   The major version number.
        /// </param>
        /// <param name="minor">
        ///   The minor version number.
        /// </param>
        /// <param name="build">
        ///   The build number.
        /// </param>
        private VersionNumber(int major, int minor, int build)
            : this(major, minor)
        {
            m_version[VersionComponent.Build] = build;
        }

        /// <summary>
        ///   Initializes a new instance of the <c>ProjectVersion</c> class 
        ///   with the specified major and minor numbers.
        /// </summary>
        /// <param name="major">
        ///   The major version number.
        /// </param>
        /// <param name="minor">
        ///   The minor version number.
        /// </param>
        private VersionNumber(int major, int minor)
            : this()
        {
            m_version[VersionComponent.Major] = major;
            m_version[VersionComponent.Minor] = minor;
        }

        /// <summary>
        ///   Copy constructor.
        /// </summary>
        /// <param name="version">
        ///   <c>ProjectVersion</c> to copy.
        /// </param>
        private VersionNumber(VersionNumber version)
            : this()
        {
            foreach (VersionComponent vc in version.Version.Keys)
            {
                m_version[vc] = (int) version.Version[vc];
            }
            Valid = version.Valid;
            m_originalString = version.m_originalString;
        }

        #endregion // Constructors

        #region IComparable interface implementation

        /// <summary>
        ///   IComparable interface implementation.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            return CompareTo((VersionNumber) obj);
        }

        /// <summary>
        ///   Compares this instance with another one.
        /// </summary>
        /// <param name="pattern">
        ///   <c>ProjectVersion</c> to compare to.
        /// </param>
        /// <returns>
        ///   0 if versions are equal, negative number if this instance 
        ///   is a lower version or positive number if this instance is a 
        ///   higher version than the version provided.
        /// </returns>
        public int CompareTo(VersionNumber other)
        {
            int[] vc1 = ComparableComponents;
            int[] vc2 = other.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] < vc2[i])
                {
                    return -1;
                }
                if (vc1[i] > vc2[i])
                {
                    return +1;
                }
            }
            return 0;
        }

        /// <summary>
        ///   Compares this version with a string presentation of another
        ///   instance.
        /// </summary>
        /// <param name="other">
        ///   String presentation of the <c>ProjectVersion</c> to compare to.
        /// </param>
        /// <returns>
        ///   0 if versions are equal, negative number if this instance 
        ///   is a lower version or positive number if this instance is a 
        ///   higher version than the version provided.
        /// </returns>
        public int CompareTo(string other)
        {
            VersionNumber otherVersion = new VersionNumber(other);
            return CompareTo(otherVersion);
        }


        /// <summary>
        ///   Checks if version provided is higher than the crrent instance. In
        ///   contrast to <c>CompareTo</c> methods, for a version component 
        ///   containing asterisk, the pattern provided is assumed to be higher.
        /// </summary>
        /// <param name="pattern">
        ///   String presentation of a version.
        /// </param>
        /// <returns>
        ///   <c>true</c> if pattern provided is higher version.
        /// </returns>
        public bool IsStringPatternHigher(string pattern)
        {
            VersionNumber other = new VersionNumber(pattern);
            int[] vc1 = ComparableComponents;
            int[] vc2 = other.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] != vc2[i])
                    return vc2[i] > vc1[i];
                // if component is asterisk, then string pattern will be higher
                if (vc1[i] == MaxVersion && vc2[i] == MaxVersion)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///   Compares this version with a pattern provided.
        /// </summary>
        /// <param name="pattern">
        ///   String pattern to compare to. Pattern may contain asterisk ('*') 
        ///   and '+' characters which are treated as wildcards. For '*' 
        ///   wildcard corresponding version component is assumed to be equal.
        ///   For '+' version component is assumed to be lower.
        /// </param>
        /// <returns>
        ///   0 if versions are equal, negative non-zero integer if this version 
        ///   is lower, positive non-zero integer if this version is higher.
        /// </returns>
        public int CompareToPattern(string pattern)
        {
            Debug.Assert(pattern != null && pattern.Length > 0);
            string[] splitPattern = pattern.Split('.');
            Debug.Assert(splitPattern.Length == 4);
            int[] components = ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (splitPattern[i] == "+")
                    return -1;
                if (splitPattern[i] != "*")
                {
                    int patternComponent = int.Parse(splitPattern[i]);
                    if (components[i] < patternComponent)
                        return -1;
                    if (components[i] > patternComponent)
                        return +1;
                }
            }
            return 0;
        }

        #endregion // IComparable interface implementation

        #region ICloneable interface implementation

        object ICloneable.Clone()
        {
            return Clone();
        }

        public VersionNumber Clone()
        {
            return new VersionNumber(this);
        }

        /// <summary>
        ///   Clones this version assigning provided Build and Revision values.
        /// </summary>
        /// <param name="build">
        ///   Build to assign.
        /// </param>
        /// <param name="revision">
        ///   Revision to assign.
        /// </param>
        /// <returns>
        ///   New <c>ProjectVersion</c> object.
        /// </returns>
        public VersionNumber Clone(int build, int revision, bool createRevision)
        {
            Debug.Assert(m_version.Count >= 2);
            if (m_version.Count == 2)
                return new VersionNumber(this[VersionComponent.Major], this[VersionComponent.Minor]);
            if (m_version.Count == 4 || (this[VersionComponent.Build] == MaxVersion && createRevision))
                return new VersionNumber(this[VersionComponent.Major], this[VersionComponent.Minor], build, revision);
            return new VersionNumber(this[VersionComponent.Major], this[VersionComponent.Minor], build);
        }

        #endregion // ICloneable interface implementation

        /// <summary>
        ///   Maximum version for a component.
        /// </summary>
        public const int MaxVersion = UInt16.MaxValue;

        #region Public methods

        /// <summary>
        ///   Checks if version contains a wildcard ('*') character.
        /// </summary>
        /// <returns>
        ///   Returns <c>true</c> if there is a wildcard character.
        /// </returns>
        public bool ContainsWildCard()
        {
            if (Version[VersionComponent.Build] != null && this[VersionComponent.Build] == MaxVersion)
                return true;
            if (Version[VersionComponent.Revision] != null && this[VersionComponent.Revision] == MaxVersion)
                return true;
            return false;
        }

        #endregion // Public methods

        #region Public overrides

        /// <summary>
        ///   Converts the value of this instance to its equivalent <c>String</c> 
        ///   representation.
        /// </summary>
        /// <returns>
        ///   The string representation of the values of the major, minor, 
        ///   build, and revision components of this instance.
        /// </returns>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        ///   Converts the value of this instance to its equivalent <c>String</c> 
        ///   representation.
        /// </summary>
        /// <param name="displayAllComponents">
        ///   Flag indicating if all versions (including Build and Revision) 
        ///   should be included. If this flag is set to <c>true</c> and Build
        ///   and/or Revision are missing, they are substituted by asterisk.
        /// </param>
        /// <returns>
        ///   The string representation of the values of the major, minor, 
        ///   build, and revision components of this instance.
        /// </returns>
        public string ToString(bool displayAllComponents)
        {
            if (this == VersionNumber.Empty)
                return m_originalString;
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}.{1}", (int) Version[VersionComponent.Major], (int) Version[VersionComponent.Minor]);
            bool asteriskInput = false;
            if (Version[VersionComponent.Build] != null)
            {
                int build = (int) Version[VersionComponent.Build];
                if (build < MaxVersion)
                {
                    Debug.Assert(build >= 0);
                    result.AppendFormat(".{0}", build);
                }
                else
                {
                    result.Append(".*");
                    asteriskInput = true;
                }
            }
            else if (displayAllComponents)
            {
                result.Append(".*");
            }
            if (Version[VersionComponent.Revision] != null)
            {
                int revision = (int) Version[VersionComponent.Revision];
                if (revision < MaxVersion)
                {
                    Debug.Assert(revision >= 0);
                    result.AppendFormat(".{0}", revision);
                }
                else if (!asteriskInput)
                {
                    result.Append(".*");
                }
            }
            else if (displayAllComponents)
            {
                result.Append(".*");
            }
            return result.ToString();
        }

        /// <summary>
        ///   Returns a value indicating whether this instance is equal to a 
        ///   specified object. 
        /// </summary>
        /// <param name="obj">
        ///   An object to compare with this instance, or a null reference.
        /// </param>
        /// <returns>
        ///   <c>true</c> if this instance and <c>obj</c> are both 
        ///   <c>ProjectVersion</c> objects, and every component of this 
        ///   instance matches the corresponding component of <c>obj</c>; 
        ///   otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            VersionNumber otherPv = (VersionNumber) obj;
            return CompareTo(otherPv) == 0;
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return 0;
        }

        #endregion Public overrides

        #region Private properties

        /// <summary>
        ///   Gets the internal <c>Version</c> list.
        /// </summary>
        private ListDictionary Version
        {
            get { return m_version; }
        }

        private int this[VersionComponent component]
        {
            get { return (int) Version[component]; }
        }

        #endregion // Private properties

        #region Private methods

        /// <summary>
        ///   Splits version components.
        /// </summary>
        /// <param name="version"></param>
        private void SplitComponents(string version)
        {
            string[] splitVersion = version.Split('.');
            Debug.Assert(splitVersion.Length > 1);
            m_version[VersionComponent.Major] = int.Parse(splitVersion[0]);
            if (splitVersion.Length > 1)
                m_version[VersionComponent.Minor] = int.Parse(splitVersion[1]);
            if (splitVersion.Length > 2)
                m_version[VersionComponent.Build] = splitVersion[2] == "*" ? MaxVersion : int.Parse(splitVersion[2]);
            if (splitVersion.Length > 3)
                m_version[VersionComponent.Revision] = splitVersion[3] == "*" ? MaxVersion : int.Parse(splitVersion[3]);
        }

        /// <summary>
        ///   Gets an array of integers representing components, that may be 
        ///   used for version comparisons.
        /// </summary>
        private int[] ComparableComponents
        {
            get
            {
                if (Version.Count == 0)
                    return new int[]
                           {
                               -1, -1, -1, -1
                           };
                int[] components = {
                    0, 0, 0, 0
                };
                Debug.Assert(Version.Count >= 2 && Version.Count <= 4);
                components[0] = (int) Version[VersionComponent.Major];
                components[1] = (int) Version[VersionComponent.Minor];
                if (Version.Count > 2)
                {
                    components[2] = (int) Version[VersionComponent.Build];
                    if (Version.Count > 3)
                    {
                        components[3] = (int) Version[VersionComponent.Revision];
                    }
                    else if (components[2] == MaxVersion)
                    {
                        components[3] = MaxVersion;
                    }
                    else
                        components[3] = 0;
                }
                return components;
            }
        }

        #endregion // Private methods

        #region Private fields

        private ListDictionary m_version;

        private string m_originalString = "";

        #endregion Private fields

        #region Public static properties

        public readonly bool Valid;

        public static readonly VersionNumber Empty = new VersionNumber();

        public static readonly VersionNumber Invalid = new VersionNumber(false);

        public static readonly VersionNumber MinValue = new VersionNumber(0, 0, 0, 0);

        #endregion // Public static properties

        #region Public static methods

        /// <summary>
        ///   Converts comma delimited string into dot delimited.
        /// </summary>
        /// <param name="version">String to convert</param>
        /// <returns>Converted string.</returns>
        public static string ConvertFromCommaDelimited(string version)
        {
            string[] versionSections = version.Split(',');
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < versionSections.Length; i++)
            {
                result.Append(versionSections[i].Trim() + ".");
            }
            return result.ToString(0, result.Length - 1);
        }

        /// <summary>
        ///   Returns higher of two <c>ProjectVersion</c> objects provided.
        /// </summary>
        /// <param name="v1">
        ///   First <c>ProjectVersion</c> to compare.
        /// </param>
        /// <param name="v2">
        ///   Second <c>ProjectVersion</c> to compare.
        /// </param>
        /// <returns>
        ///   Reference two higher <c>ProjectVersion</c> objects.
        /// </returns>
        public static VersionNumber Max(VersionNumber v1, VersionNumber v2)
        {
            int[] vc1 = v1.ComparableComponents;
            int[] vc2 = v2.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] != vc2[i])
                {
                    // if both aren't wildcards, return the larger
                    if (vc1[i] != MaxVersion && vc2[i] != MaxVersion)
                    {
                        return vc1[i] > vc2[i] ? v1 : v2;
                    }
                    else
                    {
                        return vc1[i] != MaxVersion ? v1 : v2;
                    }
                }
            }
            return v1;
        }

        /// <summary>
        ///   Checks if pattern to be applied is valid. Pattern must consist of
        ///   exactly four dot separated sections (Major, Minor, Build and 
        ///   Revision). Each section may consist of an integer, an integer
        ///   preceeded by '+' character, '*' or '+' character.
        /// </summary>
        /// <param name="pattern">
        ///   Pattern to validate.
        /// </param>
        /// <returns>
        ///   <c>true</c> if pattern is a valid one, else returns <c>false</c>.
        /// </returns>
        public static bool IsValidPattern(string pattern)
        {
            string[] patternSections = pattern.Split('.');
            if (patternSections.Length != 4)
                return false;
            foreach (string section in patternSections)
            {
                switch (section)
                {
                    case "*":
                    case "+":
                        break;
                    default:
                        try
                        {
                            long val = long.Parse(section);
                            if (val < 0 || val >= MaxVersion)
                                return false;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        #endregion // Public static methods

        #region Private static methods

        private static string IncrementStringInteger(string integerValue, int increment)
        {
            if (integerValue == "*")
                return integerValue;
            // although Parse method may throw an exception, in this case 
            // integerValue should be a valid string representation of integer
            long n = long.Parse(integerValue);
            n += increment;
            if (n >= MaxVersion)
                throw new OverflowException();
            return n.ToString();
        }

        private static bool MoreNumbersInPattern(string[] sections, int index)
        {
            for (int i = index; i < sections.Length; i++)
            {
                if (sections[i] != "*" && !sections[i].StartsWith("+"))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion Private static methods

        #region Operators

        /// <summary>
        ///   Determines whether two specified instances of <c>ProjectVersion</c> 
        ///   are equal.
        /// </summary>
        /// <param name="v1">
        ///   The first instance of <c>ProjectVersion</c>.
        /// </param>
        /// <param name="v2">
        ///   The second instance of <c>ProjectVersion</c>.
        /// </param>
        /// <returns>
        ///   <c>true</c> if v1 equals v2; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(VersionNumber v1, VersionNumber v2)
        {
            if ((object) v1 == null)
                return (object) v1 == (object) v2;
            return v1.Equals(v2);
        }

        /// <summary>
        ///   Determines whether two specified instances of <c>ProjectVersion</c> 
        ///   are not equal.
        /// </summary>
        /// <param name="v1">
        ///   The first instance of <c>ProjectVersion</c>.
        /// </param>
        /// <param name="v2">
        ///   The second instance of <c>ProjectVersion</c>.
        /// </param>
        /// <returns>
        ///   <c>true</c> if v1 does not equal v2; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(VersionNumber v1, VersionNumber v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        ///   Determines whether the first specified instance of <c>ProjectVersion</c> 
        ///   is greater than the second specified instance of <c>ProjectVersion</c>.
        /// </summary>
        /// <param name="v1">
        ///   The first instance of <c>ProjectVersion</c>.
        /// </param>
        /// <param name="v2">
        ///   The second instance of <c>ProjectVersion</c>.
        /// </param>
        /// <returns>
        ///   <c>true</c> if v1 is greater than v2; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator >(VersionNumber v1, VersionNumber v2)
        {
            Debug.Assert(v1 != null);
            Debug.Assert(v2 != null);
            int[] vc1 = v1.ComparableComponents;
            int[] vc2 = v2.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] != vc2[i])
                    return vc1[i] > vc2[i];
            }
            return false;
        }

        /// <summary>
        ///   Determines whether the first specified instance of <c>ProjectVersion</c> 
        ///   is greater than or equal to the second specified instance of 
        ///   <c>ProjectVersion</c>.
        /// </summary>
        /// <param name="v1">
        ///   The first instance of <c>ProjectVersion</c>.
        /// </param>
        /// <param name="v2">
        ///   The second instance of <c>ProjectVersion</c>.
        /// </param>
        /// <returns>
        ///   <c>true</c> if v1 is greater than or equal to v2; 
        ///   otherwise, <c>false</c>.
        /// </returns>
        public static bool operator >=(VersionNumber v1, VersionNumber v2)
        {
            Debug.Assert(v1 != null);
            Debug.Assert(v2 != null);
            int[] vc1 = v1.ComparableComponents;
            int[] vc2 = v2.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] != vc2[i])
                    return vc1[i] >= vc2[i];
            }
            return true;
        }

        /// <summary>
        ///   Determines whether the first specified instance of <c>ProjectVersion</c> 
        ///   is less than the second specified instance of 
        ///   <c>ProjectVersion</c>.
        /// </summary>
        /// <param name="v1">
        ///   The first instance of <c>ProjectVersion</c>.
        /// </param>
        /// <param name="v2">
        ///   The second instance of <c>ProjectVersion</c>.
        /// </param>
        /// <returns>
        ///   <c>true</c> if v1 is less than v2; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator <(VersionNumber v1, VersionNumber v2)
        {
            Debug.Assert(v1 != null);
            Debug.Assert(v2 != null);
            int[] vc1 = v1.ComparableComponents;
            int[] vc2 = v2.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] != vc2[i])
                    return vc1[i] < vc2[i];
            }
            return false;
        }

        /// <summary>
        ///    Determines whether the first specified instance of <c>ProjectVersion</c> 
        ///    is less than or equal to the second specified instance of 
        ///    <c>ProjectVersion</c>.
        /// </summary>
        /// <param name="v1">
        ///   The first instance of <c>ProjectVersion</c>.
        /// </param>
        /// <param name="v2">
        ///   The second instance of <c>ProjectVersion</c>.
        /// </param>
        /// <returns>
        ///   <c>true</c> if v1 is less than or equal to v2; 
        ///   otherwise, <c>false</c>.
        /// </returns>
        public static bool operator <=(VersionNumber v1, VersionNumber v2)
        {
            Debug.Assert(v1 != null);
            Debug.Assert(v2 != null);
            int[] vc1 = v1.ComparableComponents;
            int[] vc2 = v2.ComparableComponents;
            for (int i = 0; i < 4; i++)
            {
                if (vc1[i] != vc2[i])
                    return vc1[i] <= vc2[i];
            }
            return true;
        }

        #endregion // Operators

    }
}