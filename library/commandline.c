/*
Copyright E J Jaquay 2020
This file is part of VCC (Virtual Color Computer).
    VCC (Virtual Color Computer) is free software: you can redistribute it
    and/or modify it under the terms of the GNU General Public License as
    published by the Free Software Foundation, either version 3 of the License,
    or (at your option) any later version.

    VCC (Virtual Color Computer) is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VCC (Virtual Color Computer).  If not, see <http://www.gnu.org/licenses/>.
*/

//-------------------------------------------------------------------
//
// Command line arguments and options to VCC
//
// Since VCC is a windows application traditional Unix command arguments (argc,argV) 
// are not availiable. However the third argument to WinMain() does provide the 
// command string less the program name.  Historically this argument is used
// to supply the path of a Color Computer image file that is loaded into memory
// after VCC starts. 
//
// This program permits the command string to supply other information to VCC when 
// it starts up. It allows the specification of differing VCC init files. 
//
// When writing this I tried to make as flexible as possible as possible without
// adding any new dependancies to VCC. By making simple changes to this file
// and to the functions that use them it should be possible to add other things. 
// It maybe. however, that the ability to specify the vcc config file and
// possibly logging options will be sufficent for most. 
//
// Syntax:
//
// Items on command string are separated by blanks unless within quotes.
//
// Options are specified by a leading dash "-" followed by a single character 
// option code.  This may be followed by an option value. 
//
// Anything that is not an option is considered to be a positional argument.
// It is intended that the legacy image file path name will remain the first
// such argument so that existing usage is not affected.  If there is no longer
// a need to specify an image file on the command line this could change.
//
// Examples:
//
// Specify the config file fred wants to use:
// $ VCC -i C:\users\fred\my-vcc.ini
//
// Inifile in VCC appdir:
// $ VCC -i %APPDIR%/vcc/my-vcc.ini
//
// In current working directory:
// $ VCC -i my-vcc.ini
//
// Quick load image file:
// $ VCC "C:\barney's good image.ima"
//
// Errors returned:
//   CL_ERR_UNKOPT  Unknown option found
//   CL_ERR_XTRARG  Too many arguments
//
// Caveats:
//
// o Supports short (single char) option codes only. 
// o Option codes may not be combined. Each must be preceeded by dash: '-'
// o Command string cannot be longer than 255 characters.
// o Wide character or control character input is not supported.
// o No provision to escape double quotes or other special characters.
//-------------------------------------------------------------------

#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "commandline.h"

#define SEPMARK 3  //To mark spaces as separators

char* ParseCmdString(char*, const char*);
char* GetNextToken();

static char* _nextTokenPtr;

//-------------------------------------------------------------------
// Parse the VCC command string and set ConfigFile, QuickLoadFile, and 
// possibly other things.  Variables set by this routine are global and 
// are defined in commandline.h
//
// vcc.exe [-d[level]] [-i ConfigFile] [QuickLoadFile] 
//
// CmdString:  The third arg to WinMain()
//-------------------------------------------------------------------

extern "C" {
  __declspec(dllexport) CmdLineArguments __cdecl GetCmdLineArgs(char* lpCmdLine)
  {
    char* token;      // token pointer (to item in command string)
    int  argnum = 0;  // non-option argument number

    CmdLineArguments cmdArg = CmdLineArguments();

    // Options that require values need to be known by the parser
    // to allow seperation between option code and value.  
    // OPtion codes for these should be placed in the ValueRequired
    // String.  If the value is optional it can not be seperated from 
    // the code.  These should not be included in the Valurequired
    // string.

    static char* valueRequired = "i";

    // Initialize the global variables set by this routine
    cmdArg.QLoadFile[0] = '\0';
    cmdArg.IniFile[0] = '\0';

    // Get the first token from the command string
    token = ParseCmdString(lpCmdLine, valueRequired);

    while (token) {
      // Option?
      if (*token == '-') {
        switch (*(token + 1)) {
          // "-i" specfies the Vcc configuration file to use.
          // The value is required and will be found starting
          // at the third character of the option string.
          // Default config file is "vcc.ini"
        case 'i':
          strncpy(cmdArg.IniFile, token + 2, CL_MAX_PATH);
          break;

          // Unknown option code returns an error
        default:
          return cmdArg;
        }
      }
      else {
        // else Positional argument            
        // argnum indicates argument position starting at one. 
        switch (++argnum) {
          // First (currently only) positional arg is Quick Load filename.
        case 1:
        {
          strncpy(cmdArg.QLoadFile, token, CL_MAX_PATH);
          break;
        }

        // Extra positional argument returns an error
        default:
          return cmdArg;
        }
      }

      // Get next token
      token = ParseCmdString(NULL, valueRequired);
    }

    return cmdArg;
  }
}

//-------------------------------------------------------------------
// Command string parser.
//
// Input string is tokenized using one or more blanks as a separator, excepting 
// blanks within quotes. If the input String is not null a pointer to the 
// first token is returned.  Otherwise pointers to subsequent tokens are returned.  
// When there are no more tokens a null pointer is returned.
//
// ValueRequired: String containing option codes that require values.
//
//-------------------------------------------------------------------

char* ParseCmdString(char* cmdString, const char* valueRequired)
{
  static char string[256];  // Copy of cmd string
  static char option[256];  // Used to append value to option
  int quoted;
  char* token;
  char* value;

  // Initial call sets command string. Subsequent calls expect a NULL
  if (cmdString) {
    while (*cmdString == ' ') {
      cmdString++;  // Skip leading blanks
    }

    strncpy(string, cmdString, 256);        // Make a copy of what is left
    string[255] = '\0';                     // Be sure it is terminated
    _nextTokenPtr = string;                 // Save it's location

    // Mark unquoted blanks
    quoted = 0;

    for (char* p = _nextTokenPtr; *p; p++) {
      if (*p == '"') {
        quoted = !quoted;
      }
      else if (!quoted) {
        if (*p == ' ') {
          *p = SEPMARK;
        }
      }
    }
  }

  if (token = GetNextToken()) {
    // Check if it is an option token.  If a option value is
    // required and value is seperated make a copy and append
    // next token to it
    if ((token[0] == '-') && (token[1] != '\0') && (token[2] == '\0')) {
      if (strchr(valueRequired, token[1])) {
        if ((_nextTokenPtr[0] != '\0') && (_nextTokenPtr[0] != '-')) {
          if (value = GetNextToken()) {
            strcpy(option, token);
            strcat(option, value);

            return option;
          }
        }
      }
    }
  }

  return token;   // null, positional argument, or option with no value
}

//-------------------------------------------------------------------
// Terminate token, find next token, return pointer to current 
//-------------------------------------------------------------------

char* GetNextToken()
{
  // Return NULL if no tokens
  if (*_nextTokenPtr == '\0') return NULL;

  // Save token pointer 
  char* token = _nextTokenPtr++;

  // Advance to end of token
  while (*_nextTokenPtr != SEPMARK) {
    _nextTokenPtr++;
  }

  // If anything remains
  if (*_nextTokenPtr != '\0') {
    // Terminate current token
    *_nextTokenPtr++ = '\0';

    // Skip past extra separaters to start of next
    while (*_nextTokenPtr == SEPMARK) {
      _nextTokenPtr++;
    }
  }

  // Strip leading and trailing quotes from token
  if (token[0] == '\"') {
    token++;
    size_t n = strlen(token);

    if ((n > 0) && (token[n - 1] == '\"')) {
      token[n - 1] = '\0';
    }
  }

  // Return address of current token
  return token;
}
