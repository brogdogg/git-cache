FROM ubuntu:noble
# user 1000 must match the user id for the jenkins user in jenkins-master
RUN useradd -d "/var/git_cache" -u 1042 -m -s /bin/bash gitcache
USER gitcache
VOLUME ["/var/gitcache"]
CMD ["echo", "Data container for Git cache"]

