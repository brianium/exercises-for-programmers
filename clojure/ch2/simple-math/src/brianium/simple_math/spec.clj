(ns brianium.simple-math.spec
  (:require [clojure.spec.alpha :as s]))

(s/def ::operator #{#'+ #'- #'/ #'*})

(s/def ::left float?)

(s/def ::right float?)

(s/def ::result float?)

(s/def ::operation (s/keys :req-un [::operator ::left ::right ::result]))
