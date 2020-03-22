(ns brianium.madlib.spec
  (:require [clojure.spec.alpha :as s]))

(s/def ::part-of-speech #{:noun :verb :adjective :adverb})

(s/def ::ordered-part-of-speech (s/tuple ::part-of-speech int?))

(s/def ::part-of-speech-gen (s/fspec :args (s/cat :prev (s/nilable ::answer))
                                     :ret  ::ordered-part-of-speech))

(s/def ::madlib-spec (s/coll-of ::part-of-speech-gen :into []))

(s/def ::selection string?)

(s/def ::answer (s/tuple ::part-of-speech ::selection))

(s/def ::answers (s/coll-of ::answer :into []))

(s/def ::render-fn (s/fspec :args (s/cat :selections (s/coll-of ::selection) :answers ::answers
                                         :ret   string?)))
