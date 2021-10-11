# XapienTest
Solution to technical test by George Kirkman.

This solution takes a dirty list of company names from a file called `org_names.json` with duplicate entries and produces a file (output.json) containing only unique company names.
To run, build the project and run `Runner.cs`, Or run the unit tests present in `CompaniesReducerTest.cs`.

The solution reduces the example list from 8282 entries down to 2209 entries.
I'm not sure what best practice for output data is in Visual Studio solutions, but the output is written to output.json in the default directory after building and running in debug mode: `/bin/Debug/net5.0/output.json`.

Note that the function `GenerateStopWords` is unused. I have kept it because it might be the more useful solution for much larger lists of company names. In this case, business suffixes such as "llc" were not more frequent than other final words such as "grill" or "times", so a list of business suffixes was used instead.

## Edge Cases Handled
1. Case insensitive duplicates (all text was made lowercase to this end).
2. Duplicates with differences in punctuation.
3. Duplicates with one or more different business suffixes (suffixes matching an element from list `business_suffixes.json` are removed).
4. Duplicates with a leading word: "the".

## Edge Cases Not Handled
1. Spelling errors - this could perhaps be solved with similarity scores or clustering, but seems like overkill and might combine distinct companies.
2. Website URLs - could perhaps use regex to isolate domain name from url text; however, the url examples might have "wanted" the company mentioned in the article. Since there very few of these, these cases were ignored.
3. Different numbers in similar company names - in some cases these created effective duplicates (e.g. `Olswang Directors 1 Ltd` vs `Olswang Directors 2 Ltd`), while in others these were needed to differentiate company names e.g. `BBC One`, `BBC 2`. I assumed that these should remain different entries.
